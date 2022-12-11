using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
	[DataContract(IsReference = true)]
	public class User : IdentityUser
    {
        //public int Id { get; set; }
        //public string Name { get; set; } = "";
        //public string Email { get; set; } = "";
        //public string Password { get; set; } = "";

        [Required]
        public DateTime CreationTime { get; set; }

        public ICollection<Comment> CreatedComments { get; set; } = new HashSet<Comment>();
        public ICollection<Comment> LikedComments { get; set; } = new HashSet<Comment>();
        public ICollection<Comment> DislikedComments { get; set; } = new HashSet<Comment>();

        public ICollection<Event> CreatedEvents { get; set; } = new HashSet<Event>();
        public ICollection<Event> LikedEvents { get; set; } = new HashSet<Event>();
        public ICollection<Event> FavoriteEvents { get; set; } = new HashSet<Event>();

        public ICollection<Place> CreatedPlaces { get; set; } = new HashSet<Place>();
        public ICollection<Place> LikedPlaces { get; set; } = new HashSet<Place>();
        public ICollection<Place> FavoritePlaces { get; set; } = new HashSet<Place>();

        public Image? Avatar { get; set; }
        public User()
        {
            CreationTime = DateTime.Now;
        }

    }
}
