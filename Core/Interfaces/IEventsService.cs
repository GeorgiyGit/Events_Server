using Core.DTOs.EventDTOs;
using Core.DTOs.PlaceDTOs;

namespace Core.Interfaces
{
    public interface IEventsService
    {
		Task<IEnumerable<EventSimpleDTO>> GetAllSimpleAsync();
		Task<IEnumerable<EventDTO>> GetAllAsync();
		Task<EventDTO?> GetOneAsync(int id);
        Task CreateAsync(EventCreateDTO ev);
        Task EditAsync(EventEditDTO ev);
        Task DeleteAsync(int id);
    }
}
