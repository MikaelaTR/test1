using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace AdvancedProjectMVC.Models
{
    public class ApplicationUser: IdentityUser
    {
        [PersonalData]
        public string? FirstName { get; set; }

        [PersonalData]
        public string? LastName { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        public DateTime DateRegistered { get; set; }
        
        
    }
}
