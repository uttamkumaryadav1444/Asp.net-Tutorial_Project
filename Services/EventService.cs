using EventManagementWebApp.CustomExceptions;
using EventManagementWebApp.Models;
using EventManagementWebApp.Repositories;
using EventManagementWebApp.ViewModels;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System.Security.Claims;

namespace EventManagementWebApp.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IRegistirationRepository _registrationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EventService> _logger;
        private readonly IFileStorageService _fileStorageService;

        public EventService(IEventRepository eventRepository, IHttpContextAccessor httpContextAccessor, 
            ILogger<EventService> logger, IRegistirationRepository _registrationRepository
            ,IFileStorageService fileStorageService)
        {
            this._registrationRepository = _registrationRepository;
            _eventRepository = eventRepository;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        public async Task CreateEventAsync(EventViewModel model)
        {
            try
            {
                var userId = GetLoggedInUserId();

                string imagePath = null;
                if (model.Image != null && model.Image.Length > 0)
                {
                    imagePath = await _fileStorageService.UploadFileAsync(model.Image, "uploads/events");
                }

                var newEvent = new Event
                {
                    Name = model.Name,
                    Description = model.Description,
                    Date = model.Date,
                    Location = model.Location,
                    OrganizerId = userId,
                    ImagePath = imagePath 
                };

                await _eventRepository.CreateEventAsync(newEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating event.");
                throw;
            }
        }

        public async Task DeleteEventAsync(int eventId)
        {
            try
            {
                var userId = GetLoggedInUserId();
                var eventToDelete = _eventRepository.GetEventById(eventId)
                    ?? throw new NotFoundException($"Event with ID {eventId} not found.");

                if (eventToDelete.OrganizerId != userId)
                    throw new UnauthorizedAccessException("User is not the organizer of the event.");

                var result = await _eventRepository.DeleteEventAsync(eventId);

                if (result == 0)
                    throw new InvalidOperationException("Failed to delete event.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting event.");
                throw;
            }
        }

        public IEnumerable<Event> GetEvents(int numberOfEvents)
        {
            try
            {
                return _eventRepository.GetEvents(numberOfEvents);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events.");
                throw new ServiceException("Failed to fetch events.", ex);
            }
        }

        public Event GetEventById(int id)
        {
            try
            {
                return _eventRepository.GetEventById(id)
                    ?? throw new NotFoundException($"Event with ID {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching event details.");
                throw new ServiceException("An error occurred while retrieving the event.", ex);
            }
        }

        public async Task RegisterForEventAsync(int eventId, int tickets)
        {
            try
            {
                var userId = GetLoggedInUserId();

                var existingRegistration = _registrationRepository.GetRegistration(eventId, userId);

                if (existingRegistration != null)
                {
                    throw new InvalidOperationException("You have already booked this event.");
                }

                var newRegistration = new Registration
                {
                    EventId = eventId,
                    MemberId = userId,
                    Tickets = tickets
                };

                await _registrationRepository.CreateRegistrationAsync(newRegistration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering for event.");
                throw new ServiceException("An error occurred while booking the event.", ex);
            }
        }

        private int GetLoggedInUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized access: User is not logged in.");
                throw new UnauthorizedAccessException("User is not logged in.");
            }
            return int.Parse(userId);
        }

        public bool IsEventBookedByUser(int eventId)
        {
            try
            {
                var userId = GetLoggedInUserId();
                var registration = _registrationRepository.GetRegistrationByEventAndUser(eventId, userId);
                return registration != null && !registration.IsDeleted;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking event booking.");
                throw new ServiceException("Failed to check event booking.", ex);
            }
        }

        public IEnumerable<Event> GetEventsForDisplay(string name, string location, DateTime? date, int pageNumber, int pageSize)
        {
            try
            {
                return _eventRepository.GetAllEvents(name, location, date, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching events for display.");
                throw new ServiceException("Failed to fetch events for display.", ex);
            }
        }

        public int GetTotalEvents()
        {
            try
            {
                return _eventRepository.GetTotalEvents();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching total events.");
                throw new ServiceException("Failed to fetch total events.", ex);
            }
        }
    }

}
