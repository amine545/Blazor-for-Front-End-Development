namespace EventEase.Models;

public class Attendance
{
    public int EventId { get; set; }
    public string AttendeeName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Present { get; set; }
    public DateTime CheckedInAt { get; set; } = DateTime.Now;
}
