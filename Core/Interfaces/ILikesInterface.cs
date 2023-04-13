using Core.DTOs.ImageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface ILikesInterface
	{
		public Task AddPlaceLike(int id);
		public Task RemovePlaceLike(int id);
		public Task AddEventLike(int id);
		public Task RemoveEventLike(int id);

		public Task<bool> IsPlaceLike(int id);
		public Task<bool> IsEventLike(int id);
	}
}
