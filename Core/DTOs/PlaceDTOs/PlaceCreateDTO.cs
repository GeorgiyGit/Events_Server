using Core.DTOs.ImageDTOs;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.PlaceDTOs
{
    public class PlaceCreateDTO
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Route { get; set; }

        public string? Site { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string GoogleMaps { get; set; }

        public ICollection<int> Types { get; set; } = new HashSet<int>();
        //public IFormFile Image { get; set; }
        //public string Image { get; set; }
    }
}
