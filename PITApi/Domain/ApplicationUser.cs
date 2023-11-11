using Microsoft.AspNetCore.Identity;
using PTI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using PTI.Utils;

namespace PTI.Domain
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string? Name { get; set; }
        public DateTime Date { get; set; } 
        [NotMapped]
        public string? Password { get; set; }
        public string? Street { get; set; }
        public string? Complement { get; set; }
        public int? Number { get; set; }
        public string? CardNumber { get; set; }
        public int? Code { get; set; }
        public string? Validity { get; set; }

  }
}
