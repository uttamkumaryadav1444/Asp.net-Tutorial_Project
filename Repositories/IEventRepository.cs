using EventManagementWebApp.Models;

namespace EventManagementWebApp.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents(string name, string location, DateTime? date, int pageNumber, int pageSize);
        Task<int> CreateEventAsync(Event newEvent);
        Event GetEventById(int eventId);
        Task<int> DeleteEventAsync(int eventId);
        IEnumerable<Event> GetEvents(int numberOfEvents);
        int GetTotalEvents();
    }
}
