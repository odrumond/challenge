using MongoDB.Bson.Serialization.Attributes;

namespace Data.Data.Models
{
    public class AgentDto
    {
        [BsonId]
        public required Guid Id { get; set; }

        public required string Name { get; set; }

        public AgentState State { get; set; } = AgentState.UNKNOWN;

        public DateTime TimeStampUtc { get; set; }

        public Guid[] Queues { get; set; } = [];

        public virtual List<EventsDto> Events { get; set; }
    }

}