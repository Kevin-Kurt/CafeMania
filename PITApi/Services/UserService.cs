using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Utilities;

using System;
using System.Linq;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Threading.Tasks;
using PTI.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using PTI.Domain;
using keener.Models;
using PTI.Utils;
using System.Collections.Generic;
using PTI.Models;

namespace PTI.Services
{
  public class UserService
  {
    public IConfiguration configuration { get; }
    private AppDbContext _db { get; }
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signManager;

    public UserService(AppDbContext db, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IConfiguration _configuration)
    {
      configuration = _configuration;
      _signManager = signinManager;
      _db = db;
      _userManager = userManager;
    }


    public async Task<ResponseModel> AddAsync(ApplicationUser usr)
    {
      try
      {
        var findUser = await _db.Users.Where(x => x.Email == usr.Email).FirstOrDefaultAsync();
        if (findUser == null)
        {
          if (String.IsNullOrEmpty(usr.UserName) && usr.Email != null)
          {
            usr.UserName = usr.Email;
            usr.Name = usr.Email;
          }
          var result = await _userManager.CreateAsync(usr, usr.Password);
          if (result.Succeeded)
          {
            return ResponseModel.BuildOkResponse("Usuário cadastrado com sucesso");
          }
          else
          {
            return ResponseModel.BuildErrorResponse("Erro ao acessar o banco de dados");
          }
        }
        else
        {
          return ResponseModel.BuildErrorResponse("Usuário já existe");
        }
      }
      catch (Exception ex)
      {
        return ResponseModel.BuildErrorResponse(ex.Message);
      }
    }

    public async Task<ResponseModel> GetUserAsync(string userId)
    {
      try
      {
         var findUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
         return ResponseModel.BuildOkResponse(findUser);
      
      }
      catch (Exception ex)
      {
        return ResponseModel.BuildErrorResponse(ex.Message);
      }
    }
    public async Task<ResponseModel> EditUserAsync(EditModel editModel)
    {
      try
      {
        var findUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == editModel.UserId);
        findUser.Complement = editModel.Complement;
        findUser.Number = editModel.Number;
        findUser.CardNumber = editModel.CardNumber;
        findUser.Code = editModel.Code;
        findUser.Validity = editModel.Validity;
        findUser.Street = editModel.Street;

        _db.Update(findUser);
        await _db.SaveChangesAsync();

        return ResponseModel.BuildOkResponse(findUser);

      }
      catch (Exception ex)
      {
        return ResponseModel.BuildErrorResponse(ex.Message);
      }
    }


    public async Task<ResponseModel> GetTokenAsync(LoginModel usr)
    {
      try
      {

        var findUser = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == usr.Email);

        if (findUser != null)
        {
          var result = await _signManager.PasswordSignInAsync(findUser, usr.Password, false, lockoutOnFailure: false);
          if (result.Succeeded == true)
          {
            var defaultToken = GenerateToken(findUser);

            return ResponseModel.BuildOkResponse(new Models.AuthenticateUserDTO(
                new JwtSecurityTokenHandler().WriteToken(defaultToken),
                new Models.UserDTO(findUser),
                defaultToken.ValidTo,
                200
            ));
          }
          else
          {
            return ResponseModel.BuildUnauthorizedResponse("Usuario ou Senha incorretos");
          }
        }
        else
        {
          return ResponseModel.BuildUnauthorizedResponse("Usuario ou Senha incorretos");
        }
      }
      catch (Exception ex) { return ResponseModel.BuildErrorResponse(ex.Message); }
    }
    public SecurityToken GenerateToken(ApplicationUser user)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(configuration.GetSection("TokenAuthentication")["SecretKey"]);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Authentication, user.Id)
          }),
        Expires = DateTime.UtcNow.AddDays(1),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        Issuer = configuration.GetSection("TokenAuthentication")["Issuer"],
        Audience = configuration.GetSection("TokenAuthentication")["Audience"]
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      return token;
    }
  }
}
