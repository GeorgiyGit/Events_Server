using Core.DTOs.GenreDTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenresService
    {
        Task<IEnumerable<GenreDTO>> GetAllAsync();
        Task<GenreDTO?> GetOneAsync(int id);
        Task<Genre?> GetOriginalAsync(int id);
        Task Create(GenreCreateDTO ge);
        Task EditAsync(GenreEditDTO ge);
        Task DeleteAsync(int id);
    }
}
