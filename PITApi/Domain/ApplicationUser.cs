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
        public string? Nickname { get; set; }
        public string? Name { get; set; }
        public DateTime Date { get; set; } 
        [NotMapped]
        public string? Password { get; set; }
        public int WeeklyPoints { get; set; } = 0;
        public int MonthlyPoints { get; set; } = 0;
    }
}
