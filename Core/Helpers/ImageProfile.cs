using Core.DTOs.ImageDTOs;
using Core.Models;

namespace Core.Helpers
{
    internal class ImageProfile : AutoMapper.Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageDTO>();
            CreateMap<ImageDTO, Image>();
        }
    }
}
