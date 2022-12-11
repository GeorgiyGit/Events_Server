using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.CommentDTOs
{
    public class CommentEditDTO : CommentCreateDTO
    {
        public int Id { get; set; }
    }
}
