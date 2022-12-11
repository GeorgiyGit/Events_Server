using CloudinaryDotNet.Actions;
using Core.DTOs.ImageDTOs;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IImageService
    {
		//public Task<Tuple<int, string, string>> SaveImageAsync(IFormFile imageFile);
		//public bool DeleteImageAsync(string imageFileName);
		//public Task AddImageToDatabase(Image image);
		public Task AddImage(ImageCreateDTO imageDTO);
		public Task DeleteImage(int id);
		public Task<ImageDTO> GetImage(int id);
	}
}
