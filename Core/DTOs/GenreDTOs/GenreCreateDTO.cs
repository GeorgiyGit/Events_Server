using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.GenreDTOs
{
    public class GenreCreateDTO
    {
        public string Name { get; set; } = "";

        public ICollection<int> Parents { get; set; }
    }
}
