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
                if(p != null)
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

            var ge = (await repository.GetAsync(x => x.Id == id, includeProperties: $"{nameof(Genre.SubTypes)},{nameof(Genre.Parents)},{nameof(Genre.Events)},{nameof(Genre.Places)}")).First();

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
            var types = (await repository.GetAsync(includeProperties:$"{nameof(Genre.Parents)}")).ToList();
            var typesMap = mapper.Map<IEnumerable<GenreDTO>>(types).ToList();

            for(int i = 0; i < types.Count; i++)
            {
                typesMap[i].Parents = types[i].Parents.Select(p => p.Id).ToList();
            }


            return typesMap;
        }

        public async Task<GenreDTO?> GetOneAsync(int id)
        {
            var g = await GetOriginalAsync(id);
            if (g == null) return null;

            var gMap= mapper.Map<GenreDTO>(g);

            gMap.Parents = g.Parents.Select(p => p.Id).ToList();

            return gMap;
		}
        public async Task<Genre?> GetOriginalAsync(int id)
        {
			if (id < 0) throw new HttpException(ErrorMessages.GenreBadRequest, HttpStatusCode.BadRequest);

            var ev = await repository.GetAsync(x => x.Id == id, includeProperties:$"{nameof(Genre.Parents)}");


            //if (ev == null) throw new HttpException(ErrorMessages.GenreNotFound, HttpStatusCode.NotFound);

            return ev.First();
		}
    }
}
