using Core.DTOs.CommentDTOs;
using Core.Models;

namespace Core.Helpers
{
    internal class CommentProfile: AutoMapper.Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(dest => dest.OwnerUserName,
                           opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest =>dest.isLiked,
                           opt=>opt.Ignore())
				.ForMember(dest => dest.isDisliked,
						   opt => opt.Ignore());

            CreateMap<CommentDTO, Comment>();

            CreateMap<CommentCreateDTO, Comment>();

            CreateMap<CommentEditDTO, Comment>();
        }
    }
}
