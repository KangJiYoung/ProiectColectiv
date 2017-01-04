using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTaskAddViewModel
    {
        [Required]
        [Display(Name = "Template Task")]
        public int? IdDocumentTaskTemplate { get; set; }
    }
}