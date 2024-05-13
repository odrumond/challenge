using Services.Services.Models.Enums;

namespace Services.Services.Models
{
    public class CreateEventModel
    {
        public required Guid AgentId { get; set; }

        public required string AgentName { get; set; }

        public required DateTime TimeStampUtc { get; set; }

        public required string Action { get; set; }

        public Guid[]? QueuesIds { get; set; }
    }

}