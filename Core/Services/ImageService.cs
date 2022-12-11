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
		private readonly IRepository<Place> placeRepository;
		private readonly IRepository<Event> eventRepository;
		private readonly IMapper mapper;
		public ImageService(IWebHostEnvironment env,
							IMapper mapper,
							IRepository<Image> repository,
							IRepository<Place> placeRepository,
							IRepository<Event> eventRepository)
		{
			this.environment = env;
			this.repository = repository;
			this.placeRepository = placeRepository;
			this.eventRepository = eventRepository;
			this.mapper = mapper;
		}

		public bool DeleteImageAsync(string imageFileName)
		{
			var wwwPath = this.environment.WebRootPath;
			var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
			if (System.IO.File.Exists(path))
			{
				System.IO.File.Delete(path);
				return true;
			}
			return false;
		}

		public async Task<Tuple<int, string,string>> SaveImageAsync(IFormFile imageFile)
		{
			var contentPath = this.environment.ContentRootPath;
			// path = "c://projects/productminiapi/uploads" ,not exactly something like that
			var path = Path.Combine(contentPath, "Uploads");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}

			// Check the allowed extenstions
			var ext = Path.GetExtension(imageFile.FileName);
			var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
			if (!allowedExtensions.Contains(ext))
			{
				string msg = string.Format("Only {0} extensions are allowed", string.Join(",", allowedExtensions));
				return new Tuple<int, string,string>(0, msg,msg);
			}
			string uniqueString = Guid.NewGuid().ToString();
			// we are trying to create a unique filename here
			var newFileName = uniqueString + ext;
			var fileWithPath = Path.Combine(path, newFileName);
			var stream = new FileStream(fileWithPath, FileMode.Create);
			await imageFile.CopyToAsync(stream);
			stream.Close();
			return new Tuple<int, string, string>(1, newFileName, fileWithPath);
		}

		public async Task AddImageToDatabase(Image image)
		{
			if (image == null) throw new HttpException(ErrorMessages.ImageBadRequest, HttpStatusCode.BadRequest);

			await repository.AddAsync(image);
			await repository.SaveChangesAsync();
		}

		public async Task AddImage(ImageCreateDTO imageDTO)
		{
			Image img = new Image();
			var res = await SaveImageAsync(imageDTO.File);

			img.Path = res.Item3;
			img.Title = res.Item2;
			
			if (imageDTO.PlaceId != null)
			{
				var pl = await placeRepository.FindAsync(imageDTO.PlaceId);

				if(pl != null)
				{
					img.Place = pl;
					pl.Images.Add(img);
				}
			}
			else if (imageDTO.EventId != null)
			{
				var ev = await eventRepository.FindAsync(imageDTO.EventId);

				if (ev != null)
				{
					img.Event = ev;
					ev.Images.Add(img);
				}
			}

			await repository.SaveChangesAsync();
		}

		public async Task DeleteImage(int id)
		{
			var img = await repository.FindAsync(id);

			DeleteImageAsync(img.Title);
		}

		public async Task<ImageDTO> GetImage(int id)
		{
			return mapper.Map<ImageDTO>(await repository.FindAsync(id));
		}
	}
}
