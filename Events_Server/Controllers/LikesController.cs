using Core.Interfaces;
using Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events_Server.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
	[ApiController]
	public class LikesController : ControllerBase
	{
		private readonly ILikesInterface likesService;
		public LikesController(ILikesInterface likesService)
		{
			this.likesService = likesService;
		}


		[HttpPost("add_place_like/{id}")]
		public async Task<IActionResult> AddPlaceLike([FromRoute] int id)
		{
			await likesService.AddPlaceLike(id);
			return Ok();
		}

		[HttpPost("add_event_like/{id}")]
		public async Task<IActionResult> AddEventLike([FromRoute] int id)
		{
			await likesService.AddEventLike(id);
			return Ok();
		}


		[HttpDelete("remove_place_like/{id}")]
		public async Task<IActionResult> RemovePlaceLike([FromRoute] int id)
		{
			await likesService.RemovePlaceLike(id);
			return Ok();
		}

		[HttpDelete("remove_event_like/{id}")]
		public async Task<IActionResult> RemoveEventLike([FromRoute] int id)
		{
			await likesService.RemoveEventLike(id);
			return Ok();
		}


		[HttpGet("is_place_like/{id}")]
		public async Task<IActionResult> IsPlaceLike([FromRoute] int id)
		{
			return Ok(await likesService.IsPlaceLike(id));
		}

		[HttpGet("is_event_like/{id}")]
		public async Task<IActionResult> IsEventLike([FromRoute] int id)
		{
			
			return Ok(await likesService.IsEventLike(id));
		}
	}
}
