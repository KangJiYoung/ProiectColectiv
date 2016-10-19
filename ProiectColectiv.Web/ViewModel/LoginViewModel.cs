using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [MinLength(6)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
