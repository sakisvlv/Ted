using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ted.Bll.Interfaces;

namespace Ted.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkController : ControllerBase
    {
        private readonly INetworkService _networkService;

        public NetworkController(INetworkService networkService)
        {
            _networkService = networkService;
        }
    }
}