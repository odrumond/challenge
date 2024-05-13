namespace Services.Services.Interfaces
{
    public interface ITimeService
    {
        DateTime UtcNow { get; }
    }
}