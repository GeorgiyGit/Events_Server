using AutoMapper;
using Core.DTOs.GenreDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using System.Net;

namespace Core.Services
{
    public class GenresService : IGenresService
    {
		private readonly IRepository<Genre> repository;
		private readonly IMapper mapper;

        public GenresService(IRepository<Genre> repository, IMapper mapper)
        {
			this.repository = repository;
			this.mapper = mapper;
        }
        public async Task Create(GenreCreateDTO ge)
        {
            Genre g = mapper.Map<Genre>(ge);

            foreach(var id in ge.Parents)
            {
                var p = (await repository.GetAsync(g => g.Id == id, includeProperties: $"{nameof(Genre.SubTypes)}")).First();
                var exist = p.SubTypes.ToList().Find(x => x.Id == id);
                if(exist != null)
                {
                    p.SubTypes.Add(g);
                    g.Parents.Add(p);
                }
            }

            await repository.AddAsync(g);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            if (id < 0) throw new HttpException(ErrorMessages.EventBadRequest, HttpStatusCode.BadRequest);

            var ge = (await repository.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Genre.SubTypes)}")).First();

            if (ge == null) throw new HttpException(ErrorMessages.EventNotFound, HttpStatusCode.NotFound);

            repository.Remove(ge);
            await repository.SaveChangesAsync();
        }

        public async Task EditAsync(GenreEditDTO ge)
        {
			Genre g = mapper.Map<Genre>(ge);
			foreach (var id in ge.Parents)
			{
				var p = (await repository.GetAsync(g => g.Id == id, includeProperties: $"{nameof(Genre.SubTypes)}")).First();
				var exist = p.SubTypes.ToList().Find(x => x.Id == id);
				if (exist != null)
				{
					p.SubTypes.Add(g);
					g.Parents.Add(p);
				}
			}

            repository.Update(g);
            await repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<GenreDTO>> GetAllAsync()
        {
            var types = await repository.GetAllAsync();

			return mapper.Map<IEnumerable<GenreDTO>>(types);
        }

        public async Task<GenreDTO?> GetOneAsync(int id)
        {
            var g = await GetOriginalAsync(id);
            if (g == null) return null;

			return mapper.Map<GenreDTO>(g);
		}
        public async Task<Genre?> GetOriginalAsync(int id)
        {
			if (id < 0) throw new HttpException(ErrorMessages.GenreBadRequest, HttpStatusCode.BadRequest);

            var ev = await repository.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Genre.Places)},{nameof(Genre.Events)}");


            //if (ev == null) throw new HttpException(ErrorMessages.GenreNotFound, HttpStatusCode.NotFound);

            return ev.First();
		}
    }
}
