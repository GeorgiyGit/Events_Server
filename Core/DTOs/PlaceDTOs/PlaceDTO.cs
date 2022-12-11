using Core.DTOs.CommentDTOs;
using Core.DTOs.EventDTOs;
using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using Core.DTOs.UserDTOs;

namespace Core.DTOs.PlaceDTOs
{
    public class PlaceDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Text { get; set; }
        public string Route { get; set; }


        public int FullRating { get; set; }


        public string? Site { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }
        public string GoogleMaps { get; set; }

        public string OwnerId { get; set; }
        public string OwnerUserName { get; set; }

        public ICollection<CommentDTO> Comments { get; set; } = new HashSet<CommentDTO>();

        public ICollection<string> LikedUsers { get; set; } = new HashSet<string>();
        public ICollection<string> FavoriteUsers { get; set; } = new HashSet<string>();


        public ICollection<int> Events { get; set; } = new HashSet<int>();
        public ICollection<GenreBaseDTO> Types { get; set; } = new HashSet<GenreBaseDTO>();
        //public ICollection<ImageDTO> Images { get; set; } = new HashSet<ImageDTO>();
        //public string Image { get; set; }
    }
}
