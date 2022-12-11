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
	public class Image
    {
        public int Id { get; set; }

        [Required, MinLength(1)]
        public string? Title { get; set; }

        [Required, MinLength(1)]
        public string Path { get; set; } = "";
        
        public string? UserId { get; set; }
        public User? User { get; set; }

        public int? EventId { get; set; }
        public Event? Event { get; set; }

        public int? PlaceId { get; set; }
        public Place? Place { get; set; }
    }
}
