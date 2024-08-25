using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Customized
{
    public class CustomUser : IdentityUser
    {
        [StringLength(25)]
        public string? FullName { get; set; } = null!;
        public string? Phone { get; set; } = null!;
        [StringLength(100)]
        public string? Address { get; set; }

        public int? Age { get; set; }
    }
}
