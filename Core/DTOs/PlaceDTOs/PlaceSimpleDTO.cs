using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.PlaceDTOs
{
	public class PlaceSimpleDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Route { get; set; }
		public int FullRating { get; set; }
		public ICollection<ImageDTO> Images { get; set; } = new HashSet<ImageDTO>();
		public ICollection<GenreBaseDTO> Types { get; set; } = new HashSet<GenreBaseDTO>();
	}
}
