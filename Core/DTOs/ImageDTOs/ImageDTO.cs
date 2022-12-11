﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.ImageDTOs
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Path { get; set; } = "";
    }
}
