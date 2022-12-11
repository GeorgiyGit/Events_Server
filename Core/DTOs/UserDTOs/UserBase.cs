using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
    public class UserBase
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public ImageDTO? Avatar { get; set; }
    }
}
