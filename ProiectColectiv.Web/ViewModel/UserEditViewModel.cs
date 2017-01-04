using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class UserEditViewModel
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        [Display(Name = "Grup")]
        public int? IdUserGroup { get; set; }
    }
}
