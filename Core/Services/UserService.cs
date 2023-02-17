using Core.Interfaces;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> userRep;

		public UserService(IRepository<User> userRep)
		{
			this.userRep = userRep;
		}

		public async Task<User?> GetUser(string id)
		{
			return (await userRep.GetAsync(u => u.Id == id)).FirstOrDefault();	
		}
	}
}
