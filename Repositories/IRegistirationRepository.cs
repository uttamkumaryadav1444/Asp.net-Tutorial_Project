using EventManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp.Repositories
{
    public interface IRegistirationRepository
    {
        public Registration GetRegistrationByEventAndUser(int eventId, int userId);
        Registration GetRegistration(int eventId, int memberId);
        Task CreateRegistrationAsync(Registration registration);
    }
}
