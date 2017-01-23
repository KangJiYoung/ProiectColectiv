using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class DigitallySignViewModel
    {
        public int IdDocument { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile Certificate { get; set; }

        [Display(Name = "Parola")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Locatie")]
        public string Location { get; set; }

        [Display(Name = "Motiv")]
        public string Reason { get; set; }
    }
}
