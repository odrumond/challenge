using Microsoft.AspNetCore.Mvc;
using Services.Services.Exceptions;
using Services.Services.Interfaces;
using Services.Services.Models;

namespace callcenterevents.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController(IEventsService eventsService) : ControllerBase
    {
        private readonly IEventsService _eventsService = eventsService;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEventModel data)
        {
            try
            {
                var createdEvent = await _eventsService.RegisterEvent(data);
                Response.Headers.Append("location", createdEvent.ToString());
                return NoContent();
            }
            catch (Exception ex) when (ex is InvalidEventTypeException || ex is LateEventException)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
