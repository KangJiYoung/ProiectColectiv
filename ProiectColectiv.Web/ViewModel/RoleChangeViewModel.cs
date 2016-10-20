using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class RoleChangeViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
