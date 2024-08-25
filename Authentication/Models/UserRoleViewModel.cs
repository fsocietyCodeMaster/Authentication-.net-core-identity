using Authentication.Customized;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    public class UserRoleViewModel
    {
        public CustomUser CurrentUser { get; set; } 

        public List<IdentityRole> Roles { get; set; }

        public List<string> UserRoles { get; set; }
    }
}
