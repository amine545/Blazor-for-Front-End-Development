using System.ComponentModel.DataAnnotations;

namespace EventEase.Models;

public class EventModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Event name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be 3-100 chars.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date is required.")]
    [DataType(DataType.Date)]
    public DateTime Date { get; set; } = DateTime.Today;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(150, ErrorMessage = "Location too long.")]
    public string Location { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
