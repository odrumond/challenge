using Data.Data.Models;
using Data.Data.Interfaces;
using Services.Services.Models;
using Services.Services.Interfaces;
using Services.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Services.Services.Implementations
{
    public class EventsService(ICallCenterDbContext dbContext, ITimeService timeService) : IEventsService
    {
        private readonly ICallCenterDbContext _dbContext = dbContext;

        private readonly ITimeService _timeService = timeService;

        public async Task<Guid> RegisterEvent(CreateEventModel _event)
        {
            ValidateEventAge(_event);

            var agentState = GetAgentStateFromEventAction(_event);

            using (_dbContext)
            {
                var agent = await GetAgentDto(_event);
                agent.State = agentState;
                agent.Queues = _event.QueuesIds ?? [];
                agent.TimeStampUtc = _event.TimeStampUtc;

                var newEventGuid = new Guid();
                var createdEvent = new EventsDto
                {
                    AgentId = _event.AgentId,
                    Id = newEventGuid,
                    EventAction = GetEventAction(_event.Action),
                    Queues = _event.QueuesIds ?? [],
                    TimeStampUtc = _event.TimeStampUtc.ToUniversalTime()
                };

                await _dbContext.Events.AddAsync(createdEvent);
                await _dbContext.SaveChangesAsync();

                return await Task.FromResult(newEventGuid);
            }

        }

        public static AgentState GetAgentStateFromEventAction(CreateEventModel _event)
        {
            var eventAction = GetEventAction(_event.Action);

            if (eventAction == EventAction.START_DO_NOT_DISTURB && IsLunchTime(_event.TimeStampUtc))
                return AgentState.ON_LUNCH;

            if (eventAction == EventAction.CALL_STARTED)
                return AgentState.ON_CALL;

            return AgentState.UNKNOWN;
        }

        private async Task<AgentDto> GetAgentDto(CreateEventModel _event)
        {
            var agent = await _dbContext.Agents.SingleOrDefaultAsync(a => a.Id == _event.AgentId);

            if (agent is not null)
                return agent;

            var newAgent = new AgentDto
            {
                Id = _event.AgentId,
                Name = _event.AgentName,
            };

            await _dbContext.Agents.AddAsync(newAgent);
            await _dbContext.SaveChangesAsync();

            return await _dbContext.Agents.SingleAsync(a => a.Id == _event.AgentId);
        }

        private static EventAction GetEventAction(string input)
        {
            try
            {
                return (EventAction)Enum.Parse(typeof(EventAction), input);
            }
            catch (ArgumentException)
            {
                throw new InvalidEventTypeException($"Unkonwn event type: {input}");
            }
        }

        private static bool IsLunchTime(DateTime dateTime)
        {
            var eventHour = dateTime.ToUniversalTime().Hour;
            return eventHour > 11 && eventHour < 13;
        }

        private void ValidateEventAge(CreateEventModel _event)
        {
            var eventAge = _timeService.UtcNow - _event.TimeStampUtc.ToUniversalTime();

            if (eventAge.TotalHours > 1)
                throw new LateEventException("Invalid event. Event time should not be older than 1 hour.");
        }
    }
}