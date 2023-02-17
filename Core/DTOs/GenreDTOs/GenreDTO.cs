using Core.DTOs.EventDTOs;
using Core.DTOs.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GenreDTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

		public ICollection<int> Parents { get; set; } = new HashSet<int>();
	}
}
