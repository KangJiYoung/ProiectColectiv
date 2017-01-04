using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class UserGroupAddViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}