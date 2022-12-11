using AutoMapper;
using Core.DTOs.CommentDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System.Net;

namespace Core.Services
{
    public class CommentsService : ICommentsService
    {
		private readonly IRepository<Comment> commentRep;
		private readonly IMapper mapper;
		private readonly IGetUserIdService getUserIdService;

		public CommentsService(IRepository<Comment> commentRep,
                               IMapper mapper,
							   IGetUserIdService getUserIdService)
        {
			this.commentRep = commentRep;
			this.mapper = mapper;
            this.getUserIdService = getUserIdService;
        }
        public async Task CreateAsync(CommentCreateDTO com)
        {
            var comment = mapper.Map<Comment>(com);
            var userId = getUserIdService.GetUserId();

            comment.OwnerId = userId;

            await commentRep.AddAsync(comment);
            await commentRep.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

            var com = await commentRep.FindAsync(id);

            if (com == null) throw new HttpException(ErrorMessages.CommentNotFound, HttpStatusCode.NotFound);

			commentRep.Remove(com);
            await commentRep.SaveChangesAsync();

		}

        public async Task EditAsync(CommentEditDTO com)
        {
            commentRep.Update(mapper.Map<Comment>(com));
			await commentRep.SaveChangesAsync();
		}

		public async Task<IEnumerable<CommentDTO>> GetAllEventAsync(int id)
		{
			if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest); ;

			var comments = await commentRep.GetAsync(c => c.EventId == id);

			return mapper.Map<IEnumerable<CommentDTO>>(comments);
		}

		public async Task<IEnumerable<CommentDTO>> GetAllPlaceAsync(int id)
		{
            if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest); ;

            var comments = await commentRep.GetAsync(c => c.PlaceId == id);

			return mapper.Map<IEnumerable<CommentDTO>>(comments);
		}

		public async Task<CommentDTO?> GetOneAsync(int id)
        {
            if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

            var com = await commentRep.FindAsync(id);


			if (com == null) throw new HttpException(ErrorMessages.CommentNotFound, HttpStatusCode.NotFound);

            return mapper.Map<CommentDTO>(com);
        }
    }
}
