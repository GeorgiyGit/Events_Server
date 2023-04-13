using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
	public class ExternalAuthDTO
	{
		public string? Provider { get; set; }
		public string? IdToken { get; set; }
	}
}
