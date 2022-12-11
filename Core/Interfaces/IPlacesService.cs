using Core.DTOs.EventDTOs;
using Core.DTOs.PlaceDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPlacesService
    {
        Task<IEnumerable<PlaceSimpleDTO>> GetAllSimpleAsync();
		Task<IEnumerable<PlaceDTO>> GetAllAsync();
		Task<PlaceDTO?> GetOneAsync(int id);
        Task CreateAsync(PlaceCreateDTO ev);
        
        Task VerifyPlace(int id);
		Task<PlaceSimpleDTO> GetUnModeratedPlaces();

		Task EditAsync(PlaceEditDTO ev);
        Task DeleteAsync(int id);
    }
}
