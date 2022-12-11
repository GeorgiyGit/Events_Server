using Core.DTOs.CommentDTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService commentsService;

        public CommentsController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

		[HttpGet("event/{id}")]
        [AllowAnonymous]
		public async Task<IActionResult> GetAllEventAsync([FromRoute] int id)
        {
            return Ok(await commentsService.GetAllEventAsync(id));
        }
		[HttpGet("place/{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetPlaceComments([FromRoute] int id)
		{
			return Ok(await commentsService.GetAllPlaceAsync(id));
		}

		[HttpGet("{id}")]
		[AllowAnonymous]
		public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            return Ok(await commentsService.GetOneAsync(id));
        }


        [HttpPost]
		public async Task<IActionResult> CreateAsync([FromBody] CommentCreateDTO comment)
        {
            await commentsService.CreateAsync(comment);

            return Ok();
        }

        [HttpPut]
		public async Task<IActionResult> Edit([FromBody] CommentEditDTO com)
        {
            await commentsService.EditAsync(com);

            return Ok();
        }

        [HttpDelete]
		public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await commentsService.DeleteAsync(id);

            return Ok();
        }
    }
}
