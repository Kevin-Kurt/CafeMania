using keener.Helpers;
using keener.Models;
using PTI.Data;
using PTI.Models;
using PTI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PTI.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace PTI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _db;
        public IConfiguration configuration { get; }

        public HealthController(IConfiguration Configuration, UserManager<ApplicationUser> userManager, AppDbContext context, SignInManager<ApplicationUser> signinManager,
           RoleManager<IdentityRole> roleManager )
        {
            configuration = Configuration;
            _userManager = userManager;
            _db = context;
            _roleManager = roleManager;

        }

        [HttpGet]
        public ActionResult Get()
        {
            return (ActionResult)new ResponseHelper().CreateResponse(ResponseModel.BuildOkResponse("Health"));
        }

        [HttpGet]
        [Route("migrate")]
        public ActionResult<string> Migrate()
        {
            try
            {
                _db.Database.Migrate();
                return "migrated";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        [HttpGet]
        [Route("admincreate")]
        public async Task<ActionResult<string>> CreateRolesAndMasterUser()
        {
            try
            {
                //adding custom roles
                string[] roleNames = { "Player", "Master" };
                IdentityResult roleResult;
                foreach (var roleName in roleNames)
                {
                    var roleExist = await _roleManager.RoleExistsAsync(roleName);
                    if (!roleExist)
                    {
                        roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                ApplicationUser poweruser = new()
                {
                    Name = "UserTeste",
                    UserName = "UserTeste",
                    Email = "teste@keener.io",
                    EmailConfirmed = true,

                };

                var createPowerUser = await _userManager.CreateAsync(poweruser, "teste123");
                if (createPowerUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(poweruser, "Master");
                }

                return "admin created";
            }
            catch (Exception e)
            {
                return e.ToString();
            }

        }

    }
}
