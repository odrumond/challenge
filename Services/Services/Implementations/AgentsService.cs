using Data.Data.Interfaces;
using Data.Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Services.Interfaces;
using Services.Services.Models;

namespace Services.Services.Implementations
{
    public class AgentsService(ICallCenterDbContext dbContext) : IAgentsService
    {
        private readonly ICallCenterDbContext _dbContext = dbContext;

        public async Task<GetAgentModel?> GetAgentByIdAsync(Guid agentId)
        {
            using (_dbContext)
            {
                var dbAgent = await _dbContext.Agents.FindAsync(agentId);
                if (dbAgent is not null)
                    return GetAgentModel.ConvertFrom(dbAgent);

                return null;

            }
        }

        public async Task<IList<GetAgentModel>> GetAgentsAsync(int skip, int take = 100)
        {
            using (_dbContext)
            {
                var agents = await _dbContext
                    .Agents
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();
                return (agents ?? []).Select(GetAgentModel.ConvertFrom).ToList();
            }
        }
    }
}