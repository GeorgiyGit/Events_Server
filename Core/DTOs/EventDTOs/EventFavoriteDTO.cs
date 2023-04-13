using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.EventDTOs
{
	public class EventFavoriteDTO
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Route { get; set; }
		public DateTime EventTime { get; set; }
		public int Price { get; set; }
		public ICollection<GenreDTO> Types { get; set; }
		public ImageDTO Image { get; set; }
	}
}
