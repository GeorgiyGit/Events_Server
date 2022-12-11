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
	public class Genre
    {
        public int Id { get; set; }


        [Required,MinLength(4)]
        public string Name { get; set; } = "";
        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
        public ICollection<Place> Places { get; set; } = new HashSet<Place>();

		public ICollection<Genre> Parents { get; set; } = new HashSet<Genre>();
        public ICollection<Genre> SubTypes { get; } = new HashSet<Genre>();
    }
}
