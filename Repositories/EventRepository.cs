using EventManagementWebApp.Data;
using EventManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> CreateEventAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            return _context.SaveChangesAsync();
        }


        public Task<int> DeleteEventAsync(int eventId)
        {
            var eventToDelete = _context.Events.FirstOrDefault(e => e.Id == eventId);

            if (eventToDelete != null)
            {
                eventToDelete.IsDeleted = true;
                return _context.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public IEnumerable<Event> GetAllEvents(string name = null, string location = null, DateTime? date = null, int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.Events
                                .Where(e => !e.IsDeleted)
                                .Include(e => e.Registrations)  
                                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(e => e.Location.Contains(location));
            }

            if (date.HasValue)
            {
                query = query.Where(e => e.Date.Date == date.Value.Date);
            }

            var pagedEvents = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return pagedEvents;
        }



        public Event GetEventById(int eventId)
        {
            return _context.Events.FirstOrDefault(e => e.Id == eventId);
        }

        public IEnumerable<Event> GetEvents(int numberOfEvents)
        {
            return _context.Events.Where(e => !e.IsDeleted).Take(numberOfEvents).ToList();
        }

        public int GetTotalEvents()
        {
            return _context.Events.Count(e => !e.IsDeleted);
        }
    }
}
