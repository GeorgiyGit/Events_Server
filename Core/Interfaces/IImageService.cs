using CloudinaryDotNet.Actions;
using Core.DTOs.ImageDTOs;
using Core.Models;
using Core.Services;
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
		public Task<Image> SaveImage(IFormFile imageFile);
		public Task AddImageToDatabase(Image image);
		public Task<List<Image>> SaveImages(ICollection<IFormFile> imageFiles);
		public Task DeleteImage(int id);
		public Task<ImageDTO> GetImage(int id);
	}
}
