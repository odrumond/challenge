using Data.Data.Models;
using Services.Services.Models;

namespace Services.Services.Interfaces
{
    public interface IAgentsService
    {
        Task<GetAgentModel?> GetAgentByIdAsync(Guid agentId);

        Task<IList<GetAgentModel>> GetAgentsAsync(int skip, int take = 100);
    }
}