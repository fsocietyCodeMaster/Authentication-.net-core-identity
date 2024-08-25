using Microsoft.EntityFrameworkCore;

namespace Authentication.Models
{
    public class LoginViewModel
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }    
        public string? ReturnUrl { get; set; } = null!;
    }
}
