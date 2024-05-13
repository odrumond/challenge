using MongoDB.Bson.Serialization.Attributes;

namespace Data.Data.Models
{
    public class EventsDto
    {
        [BsonId]
        public required Guid Id { get; set; }

        public required Guid AgentId { get; set; }

        public DateTime TimeStampUtc { get; set; }

        public EventAction EventAction { get; set; }

        public virtual AgentDto Agent { get; set; }

        public Guid[] Queues { get; set; } = [];
    }
}