using Core.DTOs.CommentDTOs;
using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.EventDTOs
{
	public class EventSimpleDTO
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime EventTime { get; set; }
		public string Route { get; set; }
		//public ImageDTO Image { get; set; }
		public int FullRating { get; set; }
		public ICollection<GenreBaseDTO> Types { get; set; } = new HashSet<GenreBaseDTO>();
	}
}
