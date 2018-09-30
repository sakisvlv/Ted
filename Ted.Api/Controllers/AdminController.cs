using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Ted.Bll.Interfaces;

namespace Ted.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly string _filePath;

        public AdminController(IConfiguration configuration, IAdminService adminService)
        {
            _adminService = adminService;
            _filePath = configuration.GetSection("filePath").Value;
        }

        [Authorize]
        [HttpGet]
        [Route("User/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            try
            {
                var adminId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adminService.GetUser(adminId, userId);
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
        [Route("Users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adminService.GetUsers(userId);
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
        [Route("Counts")]
        public async Task<IActionResult> GetCounts()
        {
            try
            {
                var userId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adminService.GetCounts(userId);
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
        [Route("DownloadPhoto/{userId}")]
        public async Task<IActionResult> DownloadPhoto(string userId)
        {
            try
            {
                var adminId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adminService.GetPhoto(adminId, userId);
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
        [Route("GetXml")]
        public async Task<IActionResult> GetXml([FromBody]List<string> userIds)
        {
            try
            {
                var adminId = User.Claims.Where(x => x.Type == "id").FirstOrDefault().Value;
                var result = await _adminService.GetXml(adminId, userIds);
                if (!result.IsSuccess())
                {
                    return result.ToErrorResponse();
                }
                var guid = Guid.NewGuid();
                var name = guid.ToString() + ".xml";
                var path = Path.Combine(_filePath, name);

                System.IO.File.Create(path).Dispose();

                System.IO.File.WriteAllText(path, result.Data);
                return Ok(guid);
            }
            catch (Exception e)
            {
                return BadRequest("Σφάλμα, Επικοινωνήστε με τον διαχειριστή");
            }

        }
    }
}