using keener.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NPOI.XWPF.Extractor;
using PTI.Data;
using PTI.Models;
using PTI.Services;
using PTI.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PTI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController
    {
        private readonly UserService _service; private readonly AppDbContext db;


        public UserController(UserService service, AppDbContext context)
        {
            _service = service;
            db = context;
        }

        [HttpPost]
        [Route("add")]
        [AllowAnonymous]
        public async Task<IActionResult> Add(ApplicationUser usr)
        {
            return new ResponseHelper().CreateResponse(await _service.AddAsync(usr));
        }

        [HttpPost]
        [Route("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginModel usr)
        {
            return new ResponseHelper().CreateResponse(await _service.GetTokenAsync(usr));
        }


    }
}
