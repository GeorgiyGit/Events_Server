using Core.DTOs.EventDTOs;
using Core.Models;

namespace Core.Helpers
{
    internal class EventProfile : AutoMapper.Profile
    {
        public EventProfile()
        {
			CreateMap<Event, EventDTO>()
				.ForMember(dest => dest.OwnerUserName,
						   opt => opt.MapFrom(src => src.Owner.UserName))
				.ForMember(dest => dest.LikedUsers,
						   opt => opt.Ignore())
				.ForMember(dest => dest.FavoriteUsers,
						   opt => opt.Ignore())
				.ForMember(dest => dest.PlaceId,
						   opt => opt.MapFrom(src => src.Place.Id));

            CreateMap<EventDTO, Event>();

            CreateMap<EventCreateDTO, Event>().ForMember(dest => dest.Types,
														 opt => opt.Ignore())
				                              .ForMember(dest => dest.LikedUsers,
														 opt => opt.Ignore())
											  .ForMember(dest => dest.FavoriteUsers,
														 opt => opt.Ignore());

            CreateMap<EventEditDTO, Event>().ForMember(dest => dest.Types,
														 opt => opt.Ignore());

			CreateMap<Event, EventSimpleDTO>().ForMember(dest => dest.Route,
														 opt => opt.MapFrom(src => src.Place.Route));
		}
    }
}
