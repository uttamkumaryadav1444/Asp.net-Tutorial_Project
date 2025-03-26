using EventManagementWebApp.Attributes;
using EventManagementWebApp.CustomExceptions;
using EventManagementWebApp.Services;
using EventManagementWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagementWebApp.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index(string name, string location, DateTime? date, int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var pagedEvents = _eventService.GetEventsForDisplay(name, location, date, pageNumber, pageSize);
                

                var totalEvents = _eventService.GetTotalEvents();

                var viewModel = new IndexViewModel
                {
                    SliderEvents = _eventService.GetEvents(5),
                    EventsTable = pagedEvents,
                    TotalEvents = totalEvents,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading the events list.");
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 500 });
            }
        }


        [HttpGet]
        [Organizer]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Organizer]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                await _eventService.CreateEventAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access during event creation.");
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 401 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the event.");
                ModelState.AddModelError(string.Empty, "Failed to create the event.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Organizer]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _eventService.DeleteEventAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Event not found with ID {EventId}", id);
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 404 });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access during event deletion.");
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 401 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the event.");
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 500 });
            }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var eventDetails = _eventService.GetEventById(id);

                var imageUrl = eventDetails.ImagePath.Replace("\\", "/");

                var isBooked = _eventService.IsEventBookedByUser(id);

                var viewModel = new EventDetailsViewModel
                {
                    Event = eventDetails,
                    IsBooked = isBooked,
                    ImageUrl = imageUrl 
                };

                return View(viewModel);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, "Event not found: {EventId}", id);
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 404 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving the event details.");
                return RedirectToAction("HandleErrorCode", "Error", new { statusCode = 500 });
            }
        }




        [HttpPost]
        [MemberAttribute]
        public async Task<IActionResult> Register(int eventId, int tickets)
        {
            try
            {
                await _eventService.RegisterForEventAsync(eventId, tickets);
                TempData["SuccessMessage"] = "You have successfully booked the event.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while booking the event. Please try again later.";
            }

            return RedirectToAction("Details", new { id = eventId });
        }

    }

}
