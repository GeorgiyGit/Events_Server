using Core.DTOs.ImageDTOs;
using Microsoft.AspNetCore.Http;

namespace Core.DTOs.EventDTOs
{
    public class EventCreateDTO
    {
        public string Title { get; set; } = "";

        public string Text { get; set; } = "";

        public string? Site { get; set; }

        public int? PlaceId { get; set; }

        public bool IsOnline { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }
        public DateTime EventTime { get; set; }

        public ICollection<int> Types { get; set; } = new HashSet<int>();

        public int Price { get; set; }

        //public ICollection<ImageDTO> Images { get; set; } = new HashSet<ImageDTO>();
        public List<IFormFile> Images { get; set; } = new List<IFormFile>();
    }
}
