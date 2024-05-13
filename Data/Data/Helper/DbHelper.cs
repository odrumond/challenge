using Bogus;
using Data.Data.Interfaces;
using Data.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Data.Helper
{
    public static class DbHelper
    {
        public static async Task InitializeAsync(ICallCenterDbContext db)
        {
            if (await db.Agents.AnyAsync())
                return;

            var faker = new Faker<AgentDto>()
                .RuleFor((prop) => prop.Id, prop => prop.Random.Guid())
                .Rules((f, v) => v.Name = f.Name.FullName())
                .Rules((f, v) => v.TimeStampUtc = DateTime.UtcNow.AddMinutes(f.Random.Number(-120 , 120)))
                .Rules((f, v) => v.Name = f.Name.FullName());

            var agents = faker.Generate(10);

            await db.Agents.AddRangeAsync(agents);
            await db.SaveChangesAsync();
        }
    }
}