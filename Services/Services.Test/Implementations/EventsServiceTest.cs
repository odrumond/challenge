using DbModels = Data.Data.Models;
using Data.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Services.Services.Exceptions;
using Services.Services.Implementations;
using Services.Services.Models;

namespace Services.Test;

[TestClass]
public class EventsServiceTest
{
    [TestMethod]
    public async Task RegisterEvent_ThrowsLateEventException()
    {
        // Arrange
        var mockDbSet = new Mock<DbSet<DbModels.EventsDto>>();
        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Events).Returns(mockDbSet.Object);

        var inputEvent = new CreateEventModel
        {
            Action = "CALL_STARTED",
            AgentId = new Guid("00000000-0000-0000-0000-000000000000"),
            AgentName = "John Doe",
            TimeStampUtc = DateTime.UtcNow.AddSeconds(-3601),
        };

        // Act
        var service = new EventsService(mockContext.Object, new TimeService());

        // Assert
        await Assert.ThrowsExceptionAsync<LateEventException>(async () => await service.RegisterEvent(inputEvent));
    }

    [TestMethod]
    public async Task RegisterEvent_ThrowsInvalidEventTypeException()
    {
        // Arrange
        var mockDbSet = new Mock<DbSet<DbModels.EventsDto>>();
        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Events).Returns(mockDbSet.Object);

        var inputEvent = new CreateEventModel
        {
            Action = "HUNG_UP",
            AgentId = new Guid("00000000-0000-0000-0000-000000000000"),
            AgentName = "John Doe",
            TimeStampUtc = DateTime.UtcNow,
        };

        // Act
        var service = new EventsService(mockContext.Object, new TimeService());

        // Assert
        await Assert.ThrowsExceptionAsync<InvalidEventTypeException>(async () => await service.RegisterEvent(inputEvent));
    }

    [TestMethod]
    public void GetAgentStateFromEventAction_ShouldReturnON_LUNCH()
    {
        // Arrange
        var eventAction = new CreateEventModel
        {
            Action = "START_DO_NOT_DISTURB",
            TimeStampUtc = new DateTime(2024, 5, 12, 12, 30, 0, DateTimeKind.Utc),
            AgentId = new Guid("00000000-0000-0000-0000-000000000000"),
            AgentName = "John Doe",
        };

        // Act
        var agentState = EventsService.GetAgentStateFromEventAction(eventAction);
        Assert.AreEqual(DbModels.AgentState.ON_LUNCH, agentState);

    }

    [TestMethod]
    public void GetAgentStateFromEventAction_ShouldReturnON_CALL()
    {
        // Arrange
        var eventAction = new CreateEventModel
        {
            Action = "CALL_STARTED",
            TimeStampUtc = new DateTime(2024, 5, 12, 9, 30, 0, DateTimeKind.Utc),
            AgentId = new Guid("00000000-0000-0000-0000-000000000000"),
            AgentName = "John Doe",
        };

        // Act
        var agentState = EventsService.GetAgentStateFromEventAction(eventAction);
        Assert.AreEqual(DbModels.AgentState.ON_CALL, agentState);

    }

    [TestMethod]
    public async Task RegisterEvent_ShouldRegisterEvent()
    {
        // Arrange
        var agentIdToFindFor = new Guid("00000000-0000-0000-0000-000000000000");

        var data = new List<DbModels.AgentDto>()
        {
            new() {
                Id = agentIdToFindFor,
                Name = "John Doe",
                State = DbModels.AgentState.UNKNOWN
            },
        };
        var mockDbSetAgents = data.AsQueryable().BuildMockDbSet();
        var mockDbSetEvents = new Mock<DbSet<DbModels.EventsDto>>();

        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Agents).Returns(mockDbSetAgents.Object);
        mockContext.Setup(m => m.Events).Returns(mockDbSetEvents.Object);

        var inputEvent = new CreateEventModel
        {
            Action = "CALL_STARTED",
            AgentId = agentIdToFindFor,
            AgentName = "John Doe",
            TimeStampUtc = DateTime.UtcNow,
        };

        // Act
        var service = new EventsService(mockContext.Object, new TimeService());
        var createdEvent = await service.RegisterEvent(inputEvent);

        // Assert
        mockDbSetEvents.Verify(m => m.AddAsync(It.IsAny<DbModels.EventsDto>(), default), Times.Once());
        mockContext.Verify(m => m.SaveChangesAsync(), Times.Once());
    }
}