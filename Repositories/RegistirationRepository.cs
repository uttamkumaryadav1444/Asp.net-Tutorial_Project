using EventManagementWebApp.Data;
using EventManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp.Repositories
{
    public class RegistirationRepository : IRegistirationRepository
    {

        private readonly AppDbContext _context;

        public RegistirationRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task CreateRegistrationAsync(Registration registration)
        {
            _context.Registrations.Add(registration);
            return _context.SaveChangesAsync();
        }

        public Registration GetRegistration(int eventId, int memberId)
        {
            return _context.Registrations
                    .FirstOrDefault(r => r.EventId == eventId && r.MemberId == memberId && !r.IsDeleted);
        }

        public Registration GetRegistrationByEventAndUser(int eventId, int userId)
        {
            return _context.Registrations
                .FirstOrDefault(r => r.EventId == eventId && r.MemberId == userId && !r.IsDeleted);
        }
    }
}
