using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentDownloadViewModel
    {
        [Required]
        [Display(Name = "Versiune Document")]
        public int? IdDocumentState { get; set; }

        public bool IsFromTemplate { get; set; }
    }
}