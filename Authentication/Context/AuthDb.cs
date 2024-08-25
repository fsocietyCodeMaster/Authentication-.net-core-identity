using Authentication.Customized;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Context
{
    public class AuthDb : IdentityDbContext<CustomUser>
    {
        public AuthDb(DbContextOptions options) : base(options)
        {
        }



     
        
    }


}
