using Core.DTOs.ImageDTOs;
using Core.Interfaces;
using Core.Models;
using Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Events_Server.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
	[ApiController]
	public class ImagesController : Controller
	{
		private readonly IImageService imagesService;
		public ImagesController(IImageService imagesService)
		{
			this.imagesService = imagesService;
		}

		[HttpPost]
		public async Task<IActionResult> AddAsync([FromForm] ImageCreateDTO imageDTO)
		{
			await imagesService.AddImage(imageDTO);
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetAsync([FromRoute] int id)
		{
			return Ok(await imagesService.GetImage(id));
		}
	}
}
