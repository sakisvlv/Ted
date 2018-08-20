using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Ted.Bll.Interfaces;
using Ted.Model.Auth;
using Ted.Model.DTO;

namespace Ted.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IAccountService accountService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginData)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginData.Email, loginData.Password, loginData.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginData.Email);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token", await _accountService.GetToken(user) }
                    });
                }

                return _accountService.Error("Login Failed");
            }
            catch (Exception)
            {
                return BadRequest("System Error, Login Failed");
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO regData)
        {
            try
            {
                var user = new User { UserName = regData.Email, Email = regData.Email, FirstName = regData.FirstName, LastName = regData.LastName, PhoneNumber = regData.PhoneNumber };

                var result = await _userManager.CreateAsync(user, regData.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return new JsonResult(new Dictionary<string, object>
                    {
                        { "access_token", await _accountService.GetToken(user) }
                    });
                }
                return _accountService.Error("Registration Failed");
            }
            catch (Exception)
            {
                return BadRequest("System Error, Registration Failed");
            }
        }

    }
}