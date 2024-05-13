using Services.Services.Interfaces;

namespace Services.Services.Implementations
{
    public class TimeService : ITimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}