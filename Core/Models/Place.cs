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
	public class Place
    {
        public int Id { get; set; }

        [Required, MinLength(1)]
        public string Name { get; set; }

        [Required, MinLength(1)]
        public string Text { get; set; }

        [Required, MinLength(1)]
        public string Route { get; set; }

        [Required, Range(0, 100000000)]
        public int Rating { get; set; }


        public string? Site { get; set; }

        public string? Facebook { get; set; }

        public string? Instagram { get; set; }

        [Required, MinLength(1)]
        public string GoogleMaps { get; set; }
        
        public string OwnerId { get; set; }
        public User Owner { get; set; }

		[Required]
		public bool IsModerated { get; set; }
		
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();

        public ICollection<User> LikedUsers { get; set; } = new HashSet<User>();
        public ICollection<User> FavoriteUsers { get; set; } = new HashSet<User>();


        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
        public ICollection<Genre> Types { get; set; } = new HashSet<Genre>();
        public ICollection<Image> Images { get; set; } = new HashSet<Image>();

    }
}
