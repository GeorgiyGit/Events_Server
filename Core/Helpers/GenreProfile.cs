using Core.DTOs.GenreDTOs;
using Core.Models;

namespace Core.Helpers
{
    internal class GenreProfile : AutoMapper.Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDTO>().ForMember(dest => dest.Parents,
												   opt => opt.Ignore())
				                        .ForMember(dest => dest.Events,
												   opt => opt.Ignore())
										.ForMember(dest => dest.Places,
												   opt => opt.Ignore());
			CreateMap<GenreDTO, Genre>();

            CreateMap<GenreCreateDTO, Genre>().ForMember(dest => dest.Parents,
														 opt => opt.Ignore());

            CreateMap<GenreEditDTO, Genre>().ForMember(dest => dest.Parents,
                                                         opt => opt.Ignore());

			CreateMap<Genre, GenreBaseDTO>();
		}
    }
}
