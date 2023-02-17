using Core.DTOs.UserDTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
	public interface IUserService
	{
		public Task<User?> GetUser(string id);
	}
}
