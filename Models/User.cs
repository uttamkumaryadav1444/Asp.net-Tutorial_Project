using EventManagementWebApp.Data.Contracts;
using Microsoft.AspNetCore.Identity;

namespace EventManagementWebApp.Models
{
    public class User : IdentityUser<int>, ISoftDelete
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateJoined { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<Registration> Registrations { get; set; }
        public ICollection<Event> CreatedEvents { get; set; }
    }
}
