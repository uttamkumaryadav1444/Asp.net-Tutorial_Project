using EventManagementWebApp.Models;

namespace EventManagementWebApp.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<Event> SliderEvents { get; set; }
        public IEnumerable<Event> EventsTable { get; set; }

        public int TotalEvents { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }

}
