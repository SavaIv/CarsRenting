using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

using static CarRenting.Data.DataConstants.User;

namespace CarRenting.Data.Models
{
    public class User : IdentityUser
    {
        [MaxLength(FullNameMaxLength)]
        public string? FullName { get; set; }    
    }
}
