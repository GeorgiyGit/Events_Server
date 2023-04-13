using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Core.DTOs.ImageDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Principal;

namespace Core.Services
{
	public class ImageService : IImageService
	{
		private IWebHostEnvironment environment;
		private readonly IRepository<Image> repository;
		private readonly IMapper mapper;
		private readonly string filesDirectory;
		public ImageService(IWebHostEnvironment env,
							IMapper mapper,
							IRepository<Image> repository,
							IConfiguration configuration)
		{
			this.environment = env;
			this.repository = repository;
			this.mapper = mapper;
			this.filesDirectory = "Uploads";//configuration.GetSection("FilesConf").GetValue("Path");
		}

		private async Task<bool> DeleteImageFile(string imagePath)
		{
			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
				return true;
			}
			return false;
		}

		public async Task<Image> SaveImage(IFormFile imageFile)
		{
			var contentPath = this.environment.ContentRootPath;
			// path = "c://projects/productminiapi/uploads" ,not exactly something like that
			var path = Path.Combine(contentPath, filesDirectory);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			// Check the allowed extenstions
			var ext = Path.GetExtension(imageFile.FileName);
			var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
			if (!allowedExtensions.Contains(ext))
			{
				throw new HttpException(ErrorMessages.ImageSave, HttpStatusCode.BadRequest);
			}
			string uniqueString = Guid.NewGuid().ToString();
			// we are trying to create a unique filename here
			var newFileName = uniqueString + ext;
			var fileWithPath = Path.Combine(path, newFileName);

			ResizeAndSaveImage(imageFile, new System.Drawing.Size(1920, 1080), path + "\\1920_" + newFileName);
			ResizeAndSaveImage(imageFile, new System.Drawing.Size(1024, 768), path + "\\1024_" + newFileName);
			ResizeAndSaveImage(imageFile, new System.Drawing.Size(320, 240), path + "\\320_" + newFileName);
			ResizeAndSaveImage(imageFile, new System.Drawing.Size(64, 64), path + "\\64_" + newFileName);

			var image = new Image
			{
				Path = newFileName,
				Title = imageFile.FileName
			};

			return image;
		}

		private void ResizeAndSaveImage(IFormFile file, System.Drawing.Size size,string path)
		{
			System.Drawing.Image image = System.Drawing.Image.FromStream(file.OpenReadStream(), true, true);

			System.Drawing.Bitmap resizedImage = new System.Drawing.Bitmap(image, size);

			resizedImage.Save(path);

		}


		public async Task<List<Image>> SaveImages(ICollection<IFormFile> imageFiles)
		{
			if(imageFiles == null) throw new HttpException(ErrorMessages.ImageBadRequest, HttpStatusCode.BadRequest);

			List<Image> images = new List<Image>();

			foreach(var file in imageFiles)
			{
				var image = await SaveImage(file);
				
				images.Add(image);
			}

			return images;
		}

		public async Task AddImageToDatabase(Image image)
		{
			if (image == null) throw new HttpException(ErrorMessages.ImageBadRequest, HttpStatusCode.BadRequest);

			await repository.AddAsync(image);
			//await repository.SaveChangesAsync();
		}

		public async Task DeleteImage(int id)
		{
			var img = await repository.FindAsync(id);

			await DeleteImageFile(img.Path);
			
			repository.Remove(img);

			await repository.SaveChangesAsync();
		}

		public async Task<ImageDTO> GetImage(int id)
		{
			return mapper.Map<ImageDTO>(await repository.FindAsync(id));
		}

	}
}
