using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProiectColectiv.Web.ViewModel
{
    public class TemplateUploadViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nume")]
        public string Name { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}