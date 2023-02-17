using Core.DTOs.CommentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICommentsService
    {
        Task<IEnumerable<CommentDTO>> GetAllPlaceAsync(int id);
		Task<IEnumerable<CommentDTO>> GetAllEventAsync(int id);
		Task<CommentDTO?> GetOneAsync(int id);
        Task CreateAsync(CommentCreateDTO ev);
        Task EditAsync(CommentEditDTO ev);
        Task DeleteAsync(int id);

        Task AddLike(int id);
        Task DeleteLike(int id);

        Task AddDisLike(int id);
        Task DeleteDisLike(int id);
    }
}
