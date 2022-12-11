using Core.DTOs.CommentDTOs;
using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using Core.DTOs.PlaceDTOs;
using Core.DTOs.UserDTOs;

namespace Core.DTOs.EventDTOs
{
    public class EventDTO
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Text { get; set; } = "";

        public string? Site { get; set; }

        public int? PlaceId { get; set; }
        //PlaceDTO? Place { get; set; }

        public bool IsOnline { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        //public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public DateTime EventTime { get; set; }

        public ICollection<GenreBaseDTO> Types { get; set; } = new HashSet<GenreBaseDTO>();

        public int Price { get; set; }

        public string OwnerId { get; set; }
        public string OwnerUserName { get; set; }

		//public int Rating { get; set; }
		public int FullRating { get; set; }

		//public bool IsModerated { get; set; }
        public ICollection<string> LikedUsers { get; set; } = new HashSet<string>();
        public ICollection<string> FavoriteUsers { get; set; } = new HashSet<string>();

        public ICollection<CommentDTO> Comments { get; set; } = new HashSet<CommentDTO>();
        //public ICollection<ImageDTO> Images { get; set; } = new HashSet<ImageDTO>();
    }
}
