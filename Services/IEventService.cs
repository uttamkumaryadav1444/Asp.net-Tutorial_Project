using EventManagementWebApp.CustomExceptions;
using EventManagementWebApp.Models;
using EventManagementWebApp.ViewModels;
using System.Security.Claims;

namespace EventManagementWebApp.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetEventsForDisplay(string name, string location, DateTime? date, int pageNumber, int pageSize);

        IEnumerable<Event> GetEvents(int numberOfEvents);

        int GetTotalEvents();

        Task CreateEventAsync(EventViewModel model);

        Task DeleteEventAsync(int eventId);

        public Event GetEventById(int id);

        public Task RegisterForEventAsync(int eventId, int tickets);

        public bool IsEventBookedByUser(int eventId);


    }
}
