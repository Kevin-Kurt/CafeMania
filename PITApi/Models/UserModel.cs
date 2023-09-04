using keener.Helpers;
using Microsoft.AspNetCore.Identity;
using PTI.Utils;
using PTI.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using PTI.Utils.Enums;

namespace PTI.Models
{

  public class PagerModel
  {
    public int Page { get; set; }
    public int Pagesize { get; set; }
    public string Order { get; set; }
    public eTypes? Filter { get; set; }

  }

  public class LoginModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class AuthenticateUserDTO
  {
    public AuthenticateUserDTO(string Token, UserDTO User, DateTime Expires, int StatusCode)
    {
      this.Token = Token;
      this.User = User;
      this.Expires = Expires;
      this.StatusCode = StatusCode;
    }

    public string Token { get; set; }
    public UserDTO User { get; set; }
    public DateTime Expires { get; set; }
    public int StatusCode { get; set; }
  }
  public class UserDTO
  {
    public UserDTO(ApplicationUser user)
    {
      this.Id = user.Id;
      this.Email = user.Email;
    }
    public string Id { get; set; }
    public string Email { get; set; }
  }
}
