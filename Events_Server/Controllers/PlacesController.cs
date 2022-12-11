using Core.DTOs.PlaceDTOs;
using Core.Interfaces;
using Core.Resources;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class PlacesController : ControllerBase
    {
        private readonly IPlacesService placesService;

        public PlacesController(IPlacesService placesService)
        {
            this.placesService = placesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetSimpleAsync()
        {
            return Ok(await placesService.GetAllSimpleAsync());
        }

        [HttpGet("full")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await placesService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            return Ok(await placesService.GetOneAsync(id));
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PlaceCreateDTO pl)
        {
            await placesService.CreateAsync(pl);

			return Ok();
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Edit([FromBody] PlaceEditDTO pl)
        {
            await placesService.EditAsync(pl);

            return Ok();
        }

        [HttpDelete("{id}")]
		public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await placesService.DeleteAsync(id);

            return Ok();
        }

		[HttpPost("verify/{id}")]
		[Authorize(Roles = UserRoles.Moderator)]
		public async Task<IActionResult> Verify([FromRoute] int id)
		{
			await placesService.VerifyPlace(id);

			return Ok();
		}

		[HttpGet("unmoderated")]
		[Authorize(Roles = UserRoles.Moderator)]
		public async Task<IActionResult> GetUnmoderatedPlaces()
		{
            await placesService.GetUnModeratedPlaces();

			return Ok();
		}
	}
}
