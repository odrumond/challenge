using Data.Data.Interfaces;
using Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace Data.Data.Contexts.Mongo
{
    public class MongoCallCenterDbContext(DbContextOptions<MongoCallCenterDbContext> options) : DbContext(options), ICallCenterDbContext
    {
        public DbSet<EventsDto> Events { get; set; }

        public DbSet<AgentDto> Agents { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<EventsDto>().ToCollection("callCenterEvents");
            builder.Entity<AgentDto>().ToCollection("agents");
        }

        public async Task<int> SaveChangesAsync() => await base.SaveChangesAsync();
    }
}