using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ted.Bll.Interfaces;

namespace Ted.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto(IFormFile file)
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                MemoryStream memoryStream = new MemoryStream();
                file.OpenReadStream().CopyTo(memoryStream);
                var byteArray = new byte[file.Length];
                byteArray = memoryStream.ToArray();
                var result = await _userService.InsertPhoto(userId, byteArray);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                return Ok(result.Data);

            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }
        }

        [Authorize]
        [HttpGet]
        [Route("DownloadPhoto")]
        public async Task<IActionResult> DownloadPhoto()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _userService.GetPhoto(userId);
                if (!result.IsSuccess())
                {
                    return Ok();
                }
                return Ok(result.Data);
            }
            catch (Exception)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }

        }
    }
}