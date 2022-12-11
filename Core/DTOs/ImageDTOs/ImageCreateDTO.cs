using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ImageDTOs
{
    public class ImageCreateDTO
    {
        public IFormFile File { get; set; }

        public int? PlaceId { get; set; }
		public int? EventId { get; set; }
	}
}
