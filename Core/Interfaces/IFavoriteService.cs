using Core.DTOs.EventDTOs;
using Core.DTOs.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IFavoriteService
	{
		public Task AddFavoritePlace(int id);
		public Task RemoveFavoritePlace(int id);
		public Task<IEnumerable<PlaceDTO>> GetFavoritePlaces();
		public Task<bool> IsFavoritePlace(int id);

		public Task AddFavoriteEvent(int id);
		public Task RemoveFavoriteEvent(int id);
		public Task<IEnumerable<EventDTO>> GetFavoriteEvents();
		public Task<bool> IsFavoriteEvent(int id);

	}
}
