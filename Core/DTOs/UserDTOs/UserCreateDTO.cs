using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.UserDTOs
{
    public class UserCreateDTO
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public ImageDTO? Avatar { get; set; }
    }
}
