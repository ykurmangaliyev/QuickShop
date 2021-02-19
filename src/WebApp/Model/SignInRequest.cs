using System.ComponentModel.DataAnnotations;

namespace QuickShop.WebApp.Model
{
    public class SignInRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
