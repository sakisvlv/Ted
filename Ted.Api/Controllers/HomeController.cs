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
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        [Authorize]
        [HttpGet]
        [Route("LastExperience")]
        public async Task<IActionResult> GetUserSkills()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _homeService.GetLastExperience(userId);
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
        [HttpPost]
        [Route("UploadImage")]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var file = Request.Form.Files.FirstOrDefault();
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                MemoryStream memoryStream = new MemoryStream();
                file.OpenReadStream().CopyTo(memoryStream);
                var byteArray = new byte[file.Length];
                byteArray = memoryStream.ToArray();
                var result = await _homeService.InsertImage(userId, byteArray);
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
    }
}