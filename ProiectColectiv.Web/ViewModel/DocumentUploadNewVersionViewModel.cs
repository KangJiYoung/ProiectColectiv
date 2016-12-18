using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentUploadNewVersionViewModel
    {
        [Required]
        public int? IdDocument { get; set; }

        [Required]
        public IFormFile File { get; set; }
    }
}