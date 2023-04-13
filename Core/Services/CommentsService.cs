using AutoMapper;
using CloudinaryDotNet.Actions;
using Core.DTOs.CommentDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace Core.Services
{
	public class CommentsService : ICommentsService
	{
		private readonly IRepository<Comment> commentRep;
		private readonly IMapper mapper;
		private readonly IGetUserIdService getUserIdService;
		private readonly IUserService userService;

		public CommentsService(IRepository<Comment> commentRep,
							   IMapper mapper,
							   IGetUserIdService getUserIdService,
							   IUserService userService)
		{
			this.commentRep = commentRep;
			this.mapper = mapper;
			this.getUserIdService = getUserIdService;
			this.userService = userService;
		}

		public async Task AddDisLike(int id)
		{
			var comment = (await commentRep.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Comment.LikedUsers)},{nameof(Comment.DislikedUsers)}")).First();

			string userId = getUserIdService.GetUserId();

			var user = comment.LikedUsers.Where(u => u.Id == userId).FirstOrDefault();
			if (user != null)
			{
				comment.LikedUsers.Remove(user);
				comment.Likes--;
			}
			var user2 = await userService.GetUser(userId);

			if (user2 != null && comment.DislikedUsers.Where(u => u.Id == userId).Count() == 0)
			{
				comment.DislikedUsers.Add(user2);
				comment.Dislikes++;
			}

			await commentRep.SaveChangesAsync();
		}
		public async Task AddLike(int id)
		{
			var comment = (await commentRep.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Comment.LikedUsers)},{nameof(Comment.DislikedUsers)}")).First();

			string userId = getUserIdService.GetUserId();

			var user = comment.DislikedUsers.Where(u => u.Id == userId).FirstOrDefault();
			if (user != null)
			{
				comment.DislikedUsers.Remove(user);
				comment.Dislikes--;
			}
			var user2 = await userService.GetUser(userId);

			if (user2 != null && comment.LikedUsers.Where(u => u.Id == userId).Count() == 0)
			{
				comment.LikedUsers.Add(user2);
				comment.Likes++;
			}
			await commentRep.SaveChangesAsync();
		}


		public async Task DeleteDisLike(int id)
		{
			var comment = (await commentRep.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Comment.DislikedUsers)}")).First();

			string userId = getUserIdService.GetUserId();

			var user = comment.DislikedUsers.Where(u => u.Id == userId).FirstOrDefault();
			if (user != null)
			{
				comment.DislikedUsers.Remove(user);
				comment.Dislikes--;
			}
		}
		public async Task DeleteLike(int id)
		{
			var comment = (await commentRep.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Comment.LikedUsers)}")).First();

			string userId = getUserIdService.GetUserId();

			var user = comment.LikedUsers.Where(u => u.Id == userId).FirstOrDefault();
			if (user != null)
			{
				comment.LikedUsers.Remove(user);
				comment.Likes--;
			}
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
			if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

			var comments = (await commentRep.GetAsync(c => c.EventId == id, includeProperties: $"{nameof(Comment.DislikedUsers)},{nameof(Comment.LikedUsers)},{nameof(Comment.Owner)}")).ToList();

			return await GetCommentsWithIsLiked(comments);
		}

		public async Task<IEnumerable<CommentDTO>> GetAllPlaceAsync(int id)
		{
			if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

			var comments = (await commentRep.GetAsync(c => c.PlaceId == id, includeProperties: $"{nameof(Comment.DislikedUsers)},{nameof(Comment.LikedUsers)},{nameof(Comment.Owner)}")).ToList();

			return await GetCommentsWithIsLiked(comments);
		}

		public async Task<CommentDTO?> GetOneAsync(int id)
		{
			if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

			var com = await commentRep.FindAsync(id);

			if (com == null) throw new HttpException(ErrorMessages.CommentNotFound, HttpStatusCode.NotFound);

			return mapper.Map<CommentDTO>(com);
		}

		public async Task<IEnumerable<CommentDTO>> GetAllChilds(int id)
		{
			if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

			var comments = (await commentRep.GetAsync(c => c.ParentId == id, includeProperties: $"{nameof(Comment.DislikedUsers)},{nameof(Comment.LikedUsers)},{nameof(Comment.Owner)}")).ToList();

			if (comments == null) throw new HttpException(ErrorMessages.CommentNotFound, HttpStatusCode.NotFound);

			return await GetCommentsWithIsLiked(comments);
		}

		public async Task<int> GetChildsCount(int id)
		{
			if (id < 0) throw new HttpException(ErrorMessages.CommentBadRequest, HttpStatusCode.BadRequest);

			var com = (await commentRep.GetAsync(c => c.Id == id, includeProperties: $"{nameof(Comment.SubComments)}")).First();

			if (com == null) throw new HttpException(ErrorMessages.CommentNotFound, HttpStatusCode.NotFound);

			return com.SubComments.Count();
		}


		private async Task<IEnumerable<CommentDTO>> GetCommentsWithIsLiked(List<Comment> comments)
		{
			var mappedComments = mapper.Map<IList<CommentDTO>>(comments);

			string? userId = getUserIdService.GetUserId();

			if (userId == null) return mappedComments;


			var user = await userService.GetUser(userId);

			if (user == null) return mappedComments;

			for (int i = 0; i < mappedComments.Count(); i++)
			{
				if (comments[i].LikedUsers.Contains(user))
				{
					mappedComments[i].isLiked = true;
				}
				else if (comments[i].DislikedUsers.Contains(user))
				{
					mappedComments[i].isDisliked = true;
				}
			}

			return mappedComments;
		}
	}
}
