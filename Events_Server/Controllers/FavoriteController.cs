using Core.DTOs.PlaceDTOs;
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
	public class FavoriteController:ControllerBase
	{
		private readonly IFavoriteService favoriteService;

		public FavoriteController(IFavoriteService favoriteService)
		{
			this.favoriteService = favoriteService;
		}

		[HttpPost("add-place/{id}")]
		public async Task<IActionResult> AddPlace([FromRoute] int id)
		{
			await favoriteService.AddFavoritePlace(id);

			return Ok();
		}

		[HttpPost("add-event/{id}")]
		public async Task<IActionResult> AddEvent([FromRoute] int id)
		{
			await favoriteService.AddFavoriteEvent(id);

			return Ok();
		}

		[HttpDelete("remove-place/{id}")]
		public async Task<IActionResult> RemovePlace([FromRoute] int id)
		{
			await favoriteService.RemoveFavoritePlace(id);

			return Ok();
		}

		[HttpDelete("remove-event/{id}")]
		public async Task<IActionResult> RemoveEvent([FromRoute] int id)
		{
			await favoriteService.RemoveFavoriteEvent(id);

			return Ok();
		}


		[HttpGet("get-places")]
		public async Task<IActionResult> GetPlaces()
		{
			return Ok(await favoriteService.GetFavoritePlaces());
		}

		[HttpGet("get-events")]
		public async Task<IActionResult> GetEvents()
		{
			return Ok(await favoriteService.GetFavoriteEvents());
		}

		[HttpGet("is-place/{id}")]
		public async Task<IActionResult> IsFavoritePlace([FromRoute] int id)
		{
			return Ok(await favoriteService.IsFavoritePlace(id));
		}

		[HttpGet("is-event/{id}")]
		public async Task<IActionResult> IsFavoriteEvent([FromRoute] int id)
		{
			return Ok(await favoriteService.IsFavoriteEvent(id));
		}
	}
}
