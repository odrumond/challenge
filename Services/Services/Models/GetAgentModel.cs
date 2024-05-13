using DbModels = Data.Data.Models;
using Services.Services.Models.Enums;

namespace Services.Services.Models
{
    public class GetAgentModel
    {
        public required string Id { get; set; }

        public required string Name { get; set; }

        public Guid[] Queues { get; set; } = [];

        public string State { get; set; } = nameof(DbModels.AgentState.UNKNOWN);

        public DateTime? TimeStampUtc { get; set; }

        public static GetAgentModel ConvertFrom(DbModels.AgentDto agentDto)
        {
            var state = Enum.GetName(typeof(DbModels.AgentState), agentDto.State);

            return new GetAgentModel
            {
                Id = agentDto.Id.ToString(),
                Name = agentDto.Name,
                Queues = agentDto.Queues,
                State = state ?? nameof(AgentState.UNKNOWN),
                TimeStampUtc = agentDto.TimeStampUtc,
            };
        }
    }
}