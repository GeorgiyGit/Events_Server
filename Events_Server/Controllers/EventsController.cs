using Core.DTOs.EventDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events_WebAPI.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService eventsService;

        public EventsController(IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSimpleAsync()
        {
            return Ok(await eventsService.GetAllSimpleAsync());
        }
		[HttpGet("full")]
		public async Task<IActionResult> GetAllAsync()
		{
			return Ok(await eventsService.GetAllAsync());
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAsync([FromRoute]int id)
        {
            return Ok(await eventsService.GetOneAsync(id));
        }


        [HttpPost]
		public async Task<IActionResult> Create([FromBody] EventCreateDTO ev)
        {
            await eventsService.CreateAsync(ev);

            return Ok();
        }

        [HttpPut]
		public async Task<IActionResult> Edit([FromBody] EventEditDTO ev)
        {
            await eventsService.EditAsync(ev);

            return Ok();
        }

        [HttpDelete]
		public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await eventsService.DeleteAsync(id);

            return Ok();
        }
    }
}
