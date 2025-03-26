using System.ComponentModel.DataAnnotations;

namespace EventManagementWebApp.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "The Name must be at most 200 characters.")]
        public string Name { get; set; }

        [StringLength(1000, ErrorMessage = "The Description must be at most 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a date for the event.")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [StringLength(300, ErrorMessage = "The Location must be at most 300 characters.")]
        public string Location { get; set; }

        public int OrganizerId { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        
    }

}
