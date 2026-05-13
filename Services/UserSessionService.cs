namespace EventEase.Services;

public class UserSessionService
{
    public string? UserName { get; private set; }
    public string? Email { get; private set; }
    public DateTime LoginTime { get; private set; }
    public DateTime LastActivity { get; private set; }
    public int PageViews { get; private set; }
    public List<int> RegisteredEventIds { get; } = new();
    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(UserName);
    public TimeSpan SessionDuration =>
        IsAuthenticated ? DateTime.Now - LoginTime : TimeSpan.Zero;

    public event Action? OnChange;

    public void SignIn(string name, string email)
    {
        UserName = name;
        Email = email;
        LoginTime = DateTime.Now;
        LastActivity = DateTime.Now;
        PageViews = 0;
        RegisteredEventIds.Clear();
        OnChange?.Invoke();
    }

    public void SignOut()
    {
        UserName = null;
        Email = null;
        PageViews = 0;
        RegisteredEventIds.Clear();
        OnChange?.Invoke();
    }

    public void TrackPageView()
    {
        if (!IsAuthenticated) return;
        PageViews++;
        LastActivity = DateTime.Now;
        OnChange?.Invoke();
    }

    public void TrackRegistration(int eventId)
    {
        if (!IsAuthenticated) return;
        if (!RegisteredEventIds.Contains(eventId))
            RegisteredEventIds.Add(eventId);
        LastActivity = DateTime.Now;
        OnChange?.Invoke();
    }
}
