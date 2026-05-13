using EventEase.Models;

namespace EventEase.Services;

public class AttendanceService
{
    private readonly List<Attendance> _records = new();

    public void Toggle(int eventId, string name, string email)
    {
        var existing = _records.FirstOrDefault(a => a.EventId == eventId && a.Email == email);
        if (existing is null)
        {
            _records.Add(new Attendance
            {
                EventId = eventId,
                AttendeeName = name,
                Email = email,
                Present = true,
                CheckedInAt = DateTime.Now
            });
        }
        else
        {
            existing.Present = !existing.Present;
            existing.CheckedInAt = DateTime.Now;
        }
    }

    public IEnumerable<Attendance> GetForEvent(int eventId) =>
        _records.Where(a => a.EventId == eventId);

    public int CountPresent(int eventId) =>
        _records.Count(a => a.EventId == eventId && a.Present);
}
