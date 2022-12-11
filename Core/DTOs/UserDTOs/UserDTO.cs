using Core.DTOs.CommentDTOs;
using Core.DTOs.EventDTOs;
using Core.DTOs.ImageDTOs;
using Core.DTOs.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        //public string Password { get; set; } = "";

        public DateTime CreationTime { get; }

        public ICollection<int> CreatedComments { get; set; } = new HashSet<int>();
        public ICollection<int> LikedComments { get; set; } = new HashSet<int>();
        public ICollection<int> DislikedComments { get; set; } = new HashSet<int>();

        public ICollection<int> CreatedEvents { get; set; } = new HashSet<int>();
        public ICollection<int> LikedEvents { get; set; } = new HashSet<int>();
        public ICollection<int> FavoriteEvents { get; set; } = new HashSet<int>();

        public ICollection<int> CreatedPlaces { get; set; } = new HashSet<int>();
        public ICollection<int> LikedPlaces { get; set; } = new HashSet<int>();
        public ICollection<int> FavoritePlaces { get; set; } = new HashSet<int>();

        public ImageDTO? Avatar { get; set; }
    }
}
