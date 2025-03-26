using EventManagementWebApp.Models;

namespace EventManagementWebApp.ViewModels
{
    public class EventDetailsViewModel
    {
        public Event Event { get; set; }
        public bool IsBooked { get; set; }

        public string ImageUrl { get; set; }
    }

}
