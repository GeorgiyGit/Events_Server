using AutoMapper;
using Core.DTOs.EventDTOs;
using Core.DTOs.ImageDTOs;
using Core.DTOs.PlaceDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System.Net;
using System.Numerics;

namespace Core.Services
{
    public class EventsService : IEventsService
    {
		private readonly IRepository<Event> repository;
		private readonly IMapper mapper;
		private readonly IGetUserIdService getUserIdService;
		private readonly IGenresService genresService;
		private readonly IImageService imageService;
		public EventsService(IRepository<Event> repository, 
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
		public async Task<IEnumerable<EventSimpleDTO>> GetAllSimpleAsync()
		{
			var events = (await repository.GetAsync(includeProperties: $"{nameof(Event.Types)},{nameof(Event.Images)}")).ToList();

			var mappedEvents = mapper.Map<List<EventSimpleDTO>>(events);

			return mappedEvents;
		}
		public async Task<IEnumerable<EventDTO>> GetAllAsync()
        {
            var events = (await repository.GetAsync(includeProperties: $"{nameof(Event.Place)},{nameof(Event.Types)},{nameof(Event.LikedUsers)},{nameof(Event.FavoriteUsers)},{nameof(Event.Images)},{nameof(Event.Comments)}")).ToList();
			var eventsDTO = mapper.Map<List<EventDTO>>(events);

			for (int i = 0; i < events.Count(); i++)
			{
				eventsDTO[i].FullRating = events[i].Rating * 100 + events[i].LikedUsers.Count();
			}

			return mapper.Map<IEnumerable<EventDTO>>(events);
        }

        public async Task<EventDTO?> GetOneAsync(int id)
        {
			var ev = await GetOriginalEvent(id);

			var mappedEvent = mapper.Map<EventDTO>(ev);

			return mappedEvent;

		}

        public async Task CreateAsync(EventCreateDTO ev)
        {
			var _event = mapper.Map<Event>(ev);
			string userId = getUserIdService.GetUserId();

			_event.OwnerId = userId;

			foreach (var id in ev.Types)
			{
				var genre = await genresService.GetOriginalAsync(id);
				if (genre != null)
				{
					_event.Types.Add(genre);
					genre.Events.Add(_event);
				}
			}

			List<Image> images = await imageService.SaveImages(ev.Images);

			foreach (var image in images)
			{
				_event.Images.Add(image);
				image.Event = _event;

				await imageService.AddImageToDatabase(image);
			}

			await repository.AddAsync(_event);
			await repository.SaveChangesAsync();
		}

        public async Task EditAsync(EventEditDTO ev)
        {
			var _event = mapper.Map<Event>(ev);
			string userId = getUserIdService.GetUserId();

			_event.OwnerId = userId;
			foreach (var id in ev.Types)
			{
				var genre = await genresService.GetOriginalAsync(id);
				if (genre != null)
				{
					_event.Types.Add(genre);
					genre.Events.Add(_event);
				}
			}

			var oldE = (await repository.GetAsync(x => x.Id == ev.Id, includeProperties: $"{nameof(Event.Images)},{nameof(Event.Types)}")).FirstOrDefault();
			repository.Remove(oldE);
			await repository.AddAsync(_event);
			await repository.SaveChangesAsync();
		}
        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new HttpException(ErrorMessages.EventBadRequest, HttpStatusCode.BadRequest);

            var ev = await repository.FindAsync(id);

            if (ev == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

            repository.Remove(ev);
            await repository.SaveChangesAsync();
        }

		private async Task<Event> GetOriginalEvent(int id)
		{
			if (id < 0) throw new HttpException(ErrorMessages.EventBadRequest, HttpStatusCode.BadRequest);

			var ev = (await repository.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Event.Owner)},{nameof(Event.Comments)},{nameof(Event.Types)},{nameof(Event.Images)}")).FirstOrDefault();

			if (ev == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

			return ev;
		}
	}
}
