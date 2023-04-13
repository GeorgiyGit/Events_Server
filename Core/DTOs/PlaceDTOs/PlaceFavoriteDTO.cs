using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.PlaceDTOs
{
	public class PlaceFavoriteDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Route { get; set; }
		public ICollection<GenreDTO> Types { get; set; }
		public ImageDTO Image { get; set; }
	}
}
