using AutoMapper;
using Core.DTOs.GenreDTOs;
using Core.DTOs.ImageDTOs;
using Core.DTOs.PlaceDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System.Net;

namespace Core.Services
{
    public class PlacesService : IPlacesService
    {
		private readonly IRepository<Place> repository;
		//private readonly IRepository<Image> imgRep;
		private readonly IMapper mapper;
		private readonly IGetUserIdService getUserIdService;
		private readonly IGenresService genresService;
		private readonly IImageService imageService;

		public PlacesService(IRepository<Place> repository,
                             IMapper mapper,
                             IGetUserIdService getUserIdService,
                             IGenresService genresService,
							 IImageService imageService)
        {
			this.repository = repository;
			this.mapper = mapper;
            this.getUserIdService = getUserIdService;
			this.genresService = genresService;
            this.imageService = imageService;
		}
        public async Task<IEnumerable<PlaceSimpleDTO>> GetAllSimpleAsync()
        {
            var places = (await repository.GetAsync(includeProperties: $"{nameof(Place.Types)}"));

            return mapper.Map<IEnumerable<PlaceSimpleDTO>>(places);
        }

		public async Task<IEnumerable<PlaceDTO>> GetAllAsync()
		{
            var places = (await repository.GetAsync(includeProperties: $"{nameof(Place.Events)},{nameof(Place.Images)},{nameof(Place.Comments)},{nameof(Place.Owner)},{nameof(Place.Types)},{ nameof(Place.LikedUsers)}")).ToList();
            var placesDTO = mapper.Map<List<PlaceDTO>>(places);

			for(int i=0;i<places.Count();i++)
            {
                foreach(var ev in places[i].Events)
                {
					placesDTO[i].Events.Add(ev.Id);
				}
				placesDTO[i].FullRating = places[i].Rating * 100 + places[i].LikedUsers.Count();
                //placesDTO[i].Image = places[i].Images.First().Path;
            }

            return placesDTO;
		}

		public async Task<PlaceDTO?> GetOneAsync(int id)
        {
			var pl = await GetOriginalPlace(id);

			var p = mapper.Map<PlaceDTO>(pl);
            //p.Image = img.Path;


            return p;
        }

        public async Task CreateAsync(PlaceCreateDTO pl)
        {
            var p = mapper.Map<Place>(pl);
            string userId = getUserIdService.GetUserId();

            p.OwnerId = userId;

            foreach(var id in pl?.Types)
            {
                var genre = await genresService.GetOriginalAsync(id);
                if (genre != null)
                {
                    p.Types.Add(genre);
                    genre.Places.Add(p);
                }
			}
            //var img = new Image()
            //{
                //Path = pl.Image,
                //Title = pl.Image,
             //   Place = p
            //};
            //p.Images.Add(img);
           // await imgRep.AddAsync(img);
			await repository.AddAsync(p);
            await repository.SaveChangesAsync();
		}

        public async Task EditAsync(PlaceEditDTO pl)
        {
			var p = mapper.Map<Place>(pl);
			string userId = getUserIdService.GetUserId();

			p.OwnerId = userId;

            foreach (var id in pl?.Types)
            {
                var genre = await genresService.GetOriginalAsync(id);
                if (genre != null)
                {
                    p.Types.Add(genre);
                    genre.Places.Add(p);
                }
            }
            var oldP = (await repository.GetAsync(x => x.Id == pl.Id, includeProperties: $"{nameof(Place.Images)},{nameof(Place.Events)},{nameof(Place.Types)}")).FirstOrDefault();
			repository.Remove(oldP);
			await repository.AddAsync(p);
            await repository.SaveChangesAsync();
		}
        public async Task DeleteAsync(int id)
        {
            var pl = GetOriginalPlace(id);

            await repository.Remove(pl);
            await repository.SaveChangesAsync();
        }

        public async Task VerifyPlace(int id)
        {
            var place = await GetOriginalPlace(id);

            place.IsModerated = true;
            await repository.SaveChangesAsync();
        }

        private async Task<Place> GetOriginalPlace(int id)
        {
			if (id < 0) throw new HttpException(ErrorMessages.PlaceBadRequest, HttpStatusCode.BadRequest);

			var pl = (await repository.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Place.Events)},{nameof(Place.Comments)}")).FirstOrDefault();

			if (pl == null) throw new HttpException(ErrorMessages.PlaceNotFound, HttpStatusCode.NotFound);

            return pl;
        }

		public async Task<PlaceSimpleDTO> GetUnModeratedPlaces()
		{
            var places = await repository.GetAsync(pl => pl.IsModerated == false);
            return mapper.Map<PlaceSimpleDTO>(places);
		}
	}
}
