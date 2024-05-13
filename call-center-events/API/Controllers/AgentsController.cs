using Microsoft.AspNetCore.Mvc;
using Services.Services.Interfaces;

namespace callcenterevents.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgentsController(IAgentsService service) : ControllerBase
    {
        private readonly IAgentsService _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] int page = 1, [FromQuery] int pageSize = 100)
        {
            var agents = await _service.GetAgentsAsync((page - 1) * pageSize, pageSize);
            return Ok(agents);
        }

        [HttpGet("{agentId}")]
        public async Task<IActionResult> GetByIdAsync(Guid agentId)
        {
            var agent = await _service.GetAgentByIdAsync(agentId);

            if (agent is null)
                return NotFound();

            return Ok(agent);
        }
    }
}
