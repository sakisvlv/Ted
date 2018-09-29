using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ted.Bll.Interfaces;
using Ted.Dal;
using Ted.Model;
using Ted.Model.Auth;

namespace Ted.Bll.Services
{
    public class AccountService : IAccountService
    {
        private readonly JwtSettings _options;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public AccountService(IOptions<JwtSettings> optionsAccessor, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _options = optionsAccessor.Value;
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public async Task<string> GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.SecretKey);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.LastOrDefault();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("full_name", user.FirstName + " " + user.LastName),
                    new Claim("id", user.Id.ToString()),
                    new Claim("role", role),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _options.Issuer,
                Audience = _options.Audience

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public JsonResult Error(string message)
        {
            return new JsonResult(message) { StatusCode = 400 };
        }
    }
}
