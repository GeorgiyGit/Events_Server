using Core.DTOs.EventDTOs;
using Core.DTOs.PlaceDTOs;
using Core.Models;
using Google.Apis.Util;

namespace Core.Helpers
{
    internal class PlaceProfile: AutoMapper.Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceDTO>()
                .ForMember(dest => dest.OwnerUserName,
                           opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.LikedUsers,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FavoriteUsers,
                           opt => opt.Ignore())
                .ForMember(dest => dest.Events,
                           opt => opt.Ignore())
                .ForMember(dest => dest.FullRating,
                           opt => opt.Ignore());

            CreateMap<Place, PlaceSimpleDTO>()
               .ForMember(dest => dest.FullRating,
                          opt => opt.Ignore());

			CreateMap<PlaceDTO, Place>();

            CreateMap<PlaceCreateDTO, Place>().ForMember(dest => dest.Types,
                                                         opt => opt.Ignore())
							                  .ForMember(dest => dest.Images,
														 opt => opt.Ignore());


            CreateMap<PlaceEditDTO, Place>().ForMember(dest => dest.Types,
                                                       opt => opt.Ignore())
                                            .ForMember(dest => dest.Images,
													   opt => opt.Ignore());

			CreateMap<Place, PlaceFavoriteDTO>()
				.ForMember(dest => dest.Image,
						   opt => opt.Ignore());
		}
    }
}
