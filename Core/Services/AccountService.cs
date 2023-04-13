using AutoMapper;
using Core.DTOs.UserDTOs;
using Core.Exceptions;
using Core.Help_elements;
using Core.Interfaces;
using Core.Models;
using Core.Resources;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

using System.Text;

namespace Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IMapper mapper;
		private readonly IConfiguration configuration;
		private readonly IConfigurationSection _googleSettings;

		public AccountService(UserManager<User> userManager,
							  SignInManager<User> signInManager,
							  RoleManager<IdentityRole> _roleManager,
							  IMapper mapper,
							  IConfiguration configuration)
		{
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.mapper = mapper;
			this._roleManager = _roleManager;
			this.configuration = configuration;

			_googleSettings = configuration.GetSection("GoogleAuthSettings");
		}

		public async Task<TokenDTO> Login(UserLoginDTO model)
		{
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
			{
				var userRoles = await userManager.GetRolesAsync(user);

				var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}

				//var token = GetToken(authClaims);

				return new TokenDTO()
				{
					Token = await GenerateTokenAsync(user),
					Roles = userRoles
				};
			}
			throw new HttpException(ErrorMessages.Unauthorized, HttpStatusCode.NotFound);
		}


		public async Task Register(UserCreateDTO model)
		{
			var userExists = await userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
				throw new HttpException(ErrorMessages.UserExists, HttpStatusCode.NotFound);

			User user = new()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName
			};
			if (model.Avatar != null) user.Avatar = mapper.Map<Image>(model.Avatar);

			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				throw new HttpException(ErrorMessages.UserCreationFailed, HttpStatusCode.InternalServerError);

			if (!await _roleManager.RoleExistsAsync(UserRoles.User))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

			if (await _roleManager.RoleExistsAsync(UserRoles.User))
			{
				await userManager.AddToRoleAsync(user, UserRoles.User);
			}

		}

		public async Task RegisterAdmin(UserCreateDTO model)
		{
			var userExists = await userManager.FindByEmailAsync(model.Email);
			if (userExists != null)
				throw new HttpException(ErrorMessages.UserExists, HttpStatusCode.NotFound);

			User user = new()
			{
				Email = model.Email,
				SecurityStamp = Guid.NewGuid().ToString(),
				UserName = model.UserName
			};
			if (model.Avatar != null) user.Avatar = mapper.Map<Image>(model.Avatar);

			var result = await userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
				throw new HttpException(ErrorMessages.UserCreationFailed, HttpStatusCode.InternalServerError);

			if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
			if (!await _roleManager.RoleExistsAsync(UserRoles.User))
				await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

			if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await userManager.AddToRoleAsync(user, UserRoles.Admin);
			}
			if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
			{
				await userManager.AddToRoleAsync(user, UserRoles.User);
			}
		}

		private async Task<string> GenerateTokenAsync(User user)
		{
			// create claims
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			};

			var roles = await userManager.GetRolesAsync(user);
			foreach (var role in roles)
			{
			    claims.Add(new Claim(ClaimTypes.Role, role));
			}

			// generate token
			var tokenHandler = new JwtSecurityTokenHandler();

			var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>();
			var key = Encoding.ASCII.GetBytes(jwtOptions.Key);

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Issuer = jwtOptions.Issuer,
				Expires = DateTime.UtcNow.AddHours(jwtOptions.Lifetime), // TODO: not working - fix
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public async Task LogOut()
        {
            await signInManager.SignOutAsync();
        }

		public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(ExternalAuthDTO externalAuth)
		{
			try
			{
				var settings = new GoogleJsonWebSignature.ValidationSettings()
				{
					Audience = new List<string>() { _googleSettings.GetSection("clientId").Value }
				};

				var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);

				return payload;
			}
			catch (Exception ex)
			{
				//log an exception
				return null;
			}
		}

		public async Task<TokenDTO> ExternalLogin(ExternalAuthDTO externalAuth)
		{
			var payload = await VerifyGoogleToken(externalAuth);
			if (payload == null)
				throw new HttpException(ErrorMessages.ExternalAuth, HttpStatusCode.BadRequest);

			var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
			var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
			if (user == null)
			{
				user = await userManager.FindByEmailAsync(payload.Email);
				if (user == null)
				{
					user = new User { Email = payload.Email, UserName = payload.Email };
					await userManager.CreateAsync(user);
					//prepare and send an email for the email confirmation
					if (await _roleManager.RoleExistsAsync(UserRoles.User))
						await userManager.AddToRoleAsync(user, UserRoles.User);

					await userManager.AddLoginAsync(user, info);
				}
				else
				{
					await userManager.AddLoginAsync(user, info);
				}
			}
			if (user == null)
				throw new HttpException(ErrorMessages.ExternalAuth, HttpStatusCode.BadRequest);

			var userRoles = await userManager.GetRolesAsync(user);

			var authClaims = new List<Claim>
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				};

			foreach (var userRole in userRoles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, userRole));
			}

			var token = await GenerateTokenAsync(user);
			return new TokenDTO
			{
				Token = token,
				Roles = userRoles
			};
		}
	}
}
