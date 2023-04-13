using Core.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAccountService
    {
        public Task Register(UserCreateDTO user);
        public Task RegisterAdmin(UserCreateDTO user);

		public Task<TokenDTO> Login(UserLoginDTO user);
        public Task<TokenDTO> ExternalLogin(ExternalAuthDTO externalAuth);

		public Task LogOut();
    }
}
