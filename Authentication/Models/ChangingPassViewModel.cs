using Authentication.Customized;

namespace Authentication.Models
{
    public class ChangingPassViewModel
    {
        public CustomUser CurrentUser { get; set; }

        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
