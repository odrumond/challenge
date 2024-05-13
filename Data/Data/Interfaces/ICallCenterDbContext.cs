
using Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Data.Interfaces
{
    public interface ICallCenterDbContext : IDisposable
    {
        public DbSet<EventsDto> Events { get; set; }

        public DbSet<AgentDto> Agents { get; set; }

        Task<int> SaveChangesAsync();
    }
}