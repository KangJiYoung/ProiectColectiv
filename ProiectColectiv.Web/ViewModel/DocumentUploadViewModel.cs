using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ProiectColectiv.Web.Application.Attributes;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentUploadViewModel
    {
        public bool IsTemplate { get; set; }

        [StringLength(100)]
        [Display(Name = "Descriere")]
        public string Descriere { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Fisier Document")]
        [Required(ErrorMessage = "Incarca un Document valid")]
        public IFormFile File { get; set; }

        [Display(Name = "Tags")]
        [EnsureOneElement(ErrorMessage = "Adaugati cel putin 1 tag")]
        public IList<string> Tags { get; set; }
    }
}
