using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Core.Models
{
	[DataContract(IsReference = true)]
	public class Event
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Text { get; set; } = "";


        public string? Site { get; set; }

        public int? PlaceId { get; set; }
        public Place? Place { get; set; }

        [Required]
        public bool IsOnline { get; set; }


        public string? Facebook { get; set; }


        public string? Instagram { get; set; }

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        [Required]
        public DateTime EventTime { get; set; }

        public ICollection<Genre> Types { get; set; } = new HashSet<Genre>();

        [Required,Range(0,10000000)]
        public int Price { get; set; }

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required, Range(0, 100000000)]
        public int Rating { get; set; }

        [Required]
        public bool IsModerated { get; set; }
        public ICollection<User> LikedUsers { get; set; } = new HashSet<User>();
        public ICollection<User> FavoriteUsers { get; set; } = new HashSet<User>();

        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Image> Images { get; set; } = new HashSet<Image>();
    }
}
