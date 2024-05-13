using DbModels = Data.Data.Models;
using Data.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Services.Services.Implementations;

namespace Services.Test;

[TestClass]
public class AgentsServiceTest
{
    [TestMethod]
    public async Task GetAgentById_ReturnsAgent()
    {
        // Arrange
        var agentIdToFindFor = new Guid("00000000-0000-0000-0000-000000000000");
        var agentFound = new DbModels.AgentDto
        {
            Id = agentIdToFindFor,
            Name = "John Doe",
            State = DbModels.AgentState.ON_CALL
        };

        var mockDbSet = new Mock<DbSet<DbModels.AgentDto>>();
        mockDbSet.Setup(m => m.FindAsync(agentIdToFindFor)).ReturnsAsync(agentFound);

        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Agents).Returns(mockDbSet.Object);

        // Act
        var service = new AgentsService(mockContext.Object);
        var agent = await service.GetAgentByIdAsync(agentIdToFindFor);

        // Assert
        Assert.IsNotNull(agent);
        mockDbSet.Verify(m => m.FindAsync(agentIdToFindFor), Times.Once());
    }

    [TestMethod]
    public async Task GetAgentById_ReturnsNull()
    {
        // Arrange
        var agentIdToFindFor = new Guid("00000000-0000-0000-0000-000000000000");
        var mockDbSet = new Mock<DbSet<DbModels.AgentDto>>();
        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Agents).Returns(mockDbSet.Object);

        // Act
        var service = new AgentsService(mockContext.Object);
        var agent = await service.GetAgentByIdAsync(agentIdToFindFor);

        // Assert
        Assert.IsNull(agent);
        mockDbSet.Verify(m => m.FindAsync(agentIdToFindFor), Times.Once());
    }

    [TestMethod]
    public async Task GetAgents_ReturnsListOfAgents()
    {
        // Arrange
        var data = new List<DbModels.AgentDto>()
        {
            new() {
                Id = new Guid("00000000-0000-0000-0000-000000000000"),
                Name = "John Doe",
                State = DbModels.AgentState.ON_CALL
            },
            new() {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Name = "Mark Ruffalo",
                State = DbModels.AgentState.ON_LUNCH
            },
        };
        var mockDbSet = data.AsQueryable().BuildMockDbSet();
        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Agents).Returns(mockDbSet.Object);

        // Act
        var service = new AgentsService(mockContext.Object);
        var agents = await service.GetAgentsAsync(0, 2);

        // Assert
        Assert.IsNotNull(agents);
        Assert.AreEqual(2, agents.Count);
    }

    [TestMethod]
    public async Task GetAgents_ReturnsEmptyList()
    {
        // Arrange
        var data = new List<DbModels.AgentDto>();
        var mockDbSet = data.AsQueryable().BuildMockDbSet();
        var mockContext = new Mock<ICallCenterDbContext>();
        mockContext.Setup(m => m.Agents).Returns(mockDbSet.Object);

        // Act
        var service = new AgentsService(mockContext.Object);
        var agents = await service.GetAgentsAsync(0, 2);

        // Assert
        Assert.IsNotNull(agents);
        Assert.AreEqual(0, agents.Count);
    }
}