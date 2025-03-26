using EventManagementWebApp.Data.Contracts;

namespace EventManagementWebApp.Models
{
    public class Event : ISoftDelete
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public string Location { get; set; }

        public int OrganizerId { get; set; }

        public virtual User Organizer { get; set; }

        public virtual ICollection<Registration> Registrations { get; set; }

        public bool IsDeleted { get; set; }

        public string ImagePath { get; set; } 
    }

}
