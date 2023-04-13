using AutoMapper;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class LikesService : ILikesInterface
	{
		private readonly IRepository<Place> placeRep;
		private readonly IRepository<Event> eventRep;
		private readonly IRepository<User> userRep;
		private readonly IGetUserIdService userIdService;
		public LikesService(IRepository<Place> placeRep,
							 IRepository<Event> eventRep,
							 IGetUserIdService userIdService,
							 IRepository<User> userRep)
		{
			this.placeRep = placeRep;
			this.eventRep = eventRep;
			this.userIdService = userIdService;
			this.userRep = userRep;
		}
		public async Task AddEventLike(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.LikedEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			var _event = (await eventRep.GetAsync(ev => ev.Id == id, includeProperties: $"{nameof(Event.Owner)},{nameof(Event.LikedUsers)}")).FirstOrDefault();

			if (_event == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.BadRequest);

			if (!user.LikedEvents.Contains(_event)) user.LikedEvents.Add(_event);

			await userRep.SaveChangesAsync();
		}

		public async Task AddPlaceLike(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.LikedPlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			var place = (await placeRep.GetAsync(pl => pl.Id == id, includeProperties: $"{nameof(Place.Owner)},{nameof(Place.LikedUsers)}")).FirstOrDefault();

			if (place == null) throw new HttpException(ErrorMessages.PlaceNotFound, HttpStatusCode.BadRequest);

			if (!user.LikedPlaces.Contains(place)) user.LikedPlaces.Add(place);

			await userRep.SaveChangesAsync();
		}

		public async Task<bool> IsEventLike(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.LikedEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			var _event = (await eventRep.GetAsync(ev => ev.Id == id, includeProperties: $"{nameof(Event.Owner)},{nameof(Event.LikedUsers)}")).FirstOrDefault();

			if (_event == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.BadRequest);

			if (user.LikedEvents.Contains(_event)) return true;

			return false;

		}

		public async Task<bool> IsPlaceLike(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.LikedPlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			var place = (await placeRep.GetAsync(pl => pl.Id == id, includeProperties: $"{nameof(Place.Owner)},{nameof(Place.LikedUsers)}")).FirstOrDefault();

			if (place == null) throw new HttpException(ErrorMessages.PlaceNotFound, HttpStatusCode.BadRequest);

			if (user.LikedPlaces.Contains(place)) return true;
			
			return false;
		}

		public async Task RemoveEventLike(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.LikedPlaces)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			var place = (await placeRep.GetAsync(pl => pl.Id == id, includeProperties: $"{nameof(Place.Owner)},{nameof(Place.LikedUsers)}")).FirstOrDefault();

			if (place == null) throw new HttpException(ErrorMessages.PlaceNotFound, HttpStatusCode.BadRequest);

			if (user.LikedPlaces.Contains(place)) user.LikedPlaces.Remove(place);

			await userRep.SaveChangesAsync();
		}

		public async Task RemovePlaceLike(int id)
		{
			var user = (await userRep.GetAsync(u => u.Id == userIdService.GetUserId(), includeProperties: $"{nameof(User.LikedEvents)}")).FirstOrDefault();

			if (user == null) throw new HttpException(ErrorMessages.UserBadId, HttpStatusCode.BadRequest);

			var _event = (await eventRep.GetAsync(ev => ev.Id == id, includeProperties: $"{nameof(Event.Owner)},{nameof(Event.LikedUsers)}")).FirstOrDefault();

			if (_event == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.BadRequest);

			if (user.LikedEvents.Contains(_event)) user.LikedEvents.Remove(_event);

			await userRep.SaveChangesAsync();
		}
	}
}
