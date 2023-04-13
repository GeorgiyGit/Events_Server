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
	public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = "";

        [Required]
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        [Required]
        public bool IsChanged { get; set; }

        public int? ParentId { get; set; }
        public Comment? Parent { get; set; }

        public ICollection<Comment> SubComments { get; } = new HashSet<Comment>();

        public string OwnerId { get; set; }
        public User Owner { get; set; }

        [Required]
        public int Likes { get; set; }

        [Required]
        public int Dislikes { get; set; }

        public ICollection<User> LikedUsers { get; set; } = new HashSet<User>();
        public ICollection<User> DislikedUsers { get; set; } = new HashSet<User>();


        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public int? PlaceId { get; set; }
        public Place? Place { get; set; }
    }
}
