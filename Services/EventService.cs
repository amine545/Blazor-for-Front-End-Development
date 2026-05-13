using EventEase.Models;

namespace EventEase.Services;

public class EventService
{
    private readonly List<EventModel> _events;
    private readonly List<RegistrationModel> _registrations = new();
    private readonly object _lock = new();

    public EventService()
    {
        _events = new List<EventModel>
        {
            new() { Id = 1, Name = "Tech Conference 2026", Date = DateTime.Today.AddDays(14), Location = "Paris, FR", Description = "Annual technology conference featuring cloud, AI, and web sessions." },
            new() { Id = 2, Name = "Corporate Gala", Date = DateTime.Today.AddDays(30), Location = "London, UK", Description = "An elegant evening of networking for executives." },
            new() { Id = 3, Name = "Startup Pitch Night", Date = DateTime.Today.AddDays(7), Location = "Berlin, DE", Description = "Watch promising startups pitch their ideas to investors." }
        };
        for (var i = 4; i <= 200; i++)
        {
            _events.Add(new EventModel
            {
                Id = i,
                Name = $"Workshop #{i}",
                Date = DateTime.Today.AddDays(i),
                Location = "Online",
                Description = "Auto-generated event used to test virtualization and performance."
            });
        }
    }

    public IReadOnlyList<EventModel> GetAll() => _events.AsReadOnly();

    public EventModel? GetById(int id) => _events.FirstOrDefault(e => e.Id == id);

    public void AddOrUpdate(EventModel evt)
    {
        if (evt is null) throw new ArgumentNullException(nameof(evt));
        if (string.IsNullOrWhiteSpace(evt.Name)) throw new ArgumentException("Event name required.");
        lock (_lock)
        {
            var existing = _events.FirstOrDefault(e => e.Id == evt.Id);
            if (existing is null)
            {
                evt.Id = _events.Count == 0 ? 1 : _events.Max(e => e.Id) + 1;
                _events.Add(evt);
            }
            else
            {
                existing.Name = evt.Name;
                existing.Date = evt.Date;
                existing.Location = evt.Location;
                existing.Description = evt.Description;
            }
        }
    }

    public void Register(RegistrationModel reg)
    {
        if (reg is null) throw new ArgumentNullException(nameof(reg));
        lock (_lock) _registrations.Add(reg);
    }

    public IEnumerable<RegistrationModel> GetRegistrations(int eventId) =>
        _registrations.Where(r => r.EventId == eventId);
}
