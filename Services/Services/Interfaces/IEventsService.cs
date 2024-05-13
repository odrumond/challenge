using Services.Services.Models;

namespace Services.Services.Interfaces
{
    public interface IEventsService
    {
        Task<Guid> RegisterEvent(CreateEventModel newEvent);
    }
}