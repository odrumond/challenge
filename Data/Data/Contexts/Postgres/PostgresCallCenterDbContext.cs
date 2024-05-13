using Data.Data.Interfaces;
using Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Data.Contexts.Postgres
{
    public class PostgresCallCenterDbContext(DbContextOptions<PostgresCallCenterDbContext> options) : DbContext(options), ICallCenterDbContext
    {
        public DbSet<EventsDto> Events { get; set; }

        public DbSet<AgentDto> Agents { get; set; }

        public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
    }
}