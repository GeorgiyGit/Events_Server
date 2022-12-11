using AutoMapper;
using Azure;
using Core.DTOs.UserDTOs;
using Core.Exceptions;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountSevice;
        public AccountController(IAccountService accountSevice)
		{
			this.accountSevice = accountSevice;
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] UserLoginDTO model)
		{
			return Ok(await accountSevice.Login(model));
		}

		[HttpPost]
		[Route("register")]
		public async Task<IActionResult> Register([FromBody] UserCreateDTO model)
		{
			await accountSevice.Register(model);
			return Ok();
		}

		[HttpPost]
		[Route("register-admin")]
		public async Task<IActionResult> RegisterAdmin([FromBody] UserCreateDTO model)
		{
			await accountSevice.RegisterAdmin(model);
			return Ok();
		}

		[HttpPost]
		[Route("logout")]
		public async Task LogOut()
		{
			await accountSevice.LogOut();
		}
	}
}
