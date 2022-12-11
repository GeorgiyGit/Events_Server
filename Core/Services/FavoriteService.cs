using AutoMapper;
using Core.DTOs.EventDTOs;
using Core.DTOs.PlaceDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class FavoriteService : IFavoriteService
	{

		private readonly IMapper mapper;
		private readonly IRepository<Place> placeRep;
		private readonly IRepository<Event> eventRep;
		private readonly IRepository<User> userRep;
		private readonly IGetUserIdService userIdService;
		public FavoriteService(IMapper mapper,
							 IRepository<Place> placeRep,
							 IRepository<Event> eventRep,
							 IGetUserIdService userIdService,
							 IRepository<User> userRep)
		{
			this.placeRep = placeRep;
			this.mapper = mapper;
			this.eventRep = eventRep;
			this.userIdService = userIdService;
			this.userRep = userRep;
		}
		public async Task AddFavoriteEvent(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoriteEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			var _event = (await eventRep.GetAsync(pl=>pl.Id== id, includeProperties: $"{nameof(Event.Owner)},{nameof(Event.FavoriteUsers)}")).FirstOrDefault();

			if (_event == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

			user.FavoriteEvents.Add(_event);
			_event.FavoriteUsers.Add(user);

			await userRep.SaveChangesAsync();
		}
		public async Task AddFavoritePlace(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoritePlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			var place = (await placeRep.GetAsync(pl=> pl.Id == id,includeProperties:$"{nameof(Place.Owner)},{nameof(Place.FavoriteUsers)}")).FirstOrDefault();

			if (place == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

			user.FavoritePlaces.Add(place);
			place.FavoriteUsers.Add(user);

			await userRep.SaveChangesAsync();
		}
		public async Task<IEnumerable<EventDTO>> GetFavoriteEvents()
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoriteEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);
			//var events = await eventRep.GetAsync();

			return mapper.Map<IEnumerable<EventDTO>>(user.FavoriteEvents);
		}
		public async Task<IEnumerable<PlaceDTO>> GetFavoritePlaces()
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoritePlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			//var places = await placeRep.GetAsync();

			//if (places == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

			return mapper.Map<IEnumerable<PlaceDTO>>(user.FavoritePlaces);
		}

		public async Task<bool> IsFavoriteEvent(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoriteEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			if(user.FavoriteEvents.ToList().Where(x => x.Id == id).Count() > 0) return true;
			return false;
		}

		public async Task<bool> IsFavoritePlace(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoritePlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			if (user.FavoritePlaces.ToList().Where(x => x.Id == id).Count() > 0) return true;
			return false;
		}

		public async Task RemoveFavoriteEvent(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoriteEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			var _event = (await eventRep.GetAsync(ev => ev.Id == id, includeProperties: $"{nameof(Event.Owner)},{nameof(Event.FavoriteUsers)}")).FirstOrDefault();

			if (_event == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);
		
			user.FavoriteEvents.Remove(_event);
			_event.FavoriteUsers.Remove(user);

			await userRep.SaveChangesAsync();
		}

		public async Task RemoveFavoritePlace(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.FavoritePlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.NotFound);

			var place = (await placeRep.GetAsync(pl => pl.Id == id, includeProperties: $"{nameof(Place.Owner)},{nameof(Place.FavoriteUsers)}")).FirstOrDefault();

			if (place == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

			user.FavoritePlaces.Remove(place);
			place.FavoriteUsers.Remove(user);

			await userRep.SaveChangesAsync();
		}
	}
}
