using Core.DTOs.EventDTOs;
using Core.DTOs.GenreDTOs;
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
    public class GenresController : ControllerBase
    {
        private readonly IGenresService genresService;

        public GenresController(IGenresService genresService)
        {
            this.genresService = genresService;
        }

        [HttpGet]
        [AllowAnonymous]
		public async Task<IActionResult> GetAsync()
        {
            return Ok(await genresService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            return Ok(await genresService.GetOneAsync(id));
        }


        [HttpPost]
		[Authorize]//(Roles = UserRoles.Admin)]
		public async Task<IActionResult> Create([FromBody] GenreCreateDTO ge)
        {
            await genresService.Create(ge);

            return Ok();
        }

        [HttpPut]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> Edit([FromBody] GenreEditDTO ge)
        {
            await genresService.EditAsync(ge);

            return Ok();
        }

        [HttpDelete("{id}")]
		[Authorize(Roles = UserRoles.Admin)]
		public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await genresService.DeleteAsync(id);

            return Ok();
        }
    }
}
