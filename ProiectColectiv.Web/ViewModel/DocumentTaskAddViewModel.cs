using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTaskAddViewModel
    {
        [Required]
        [Display(Name = "Template Task")]
        public int? IdDocumentTaskTemplate { get; set; }

        [Required]
        [Display(Name = "Tip Task")]
        public int? IdDocumentTaskType { get; set; }

        [Required]
        [Display(Name = "Document Principal")]
        public int? IdDocumentFromTemplate { get; set; }

        [Display(Name = "Documente Aditionale")]
        public IList<int> OtherDocuments { get; set; }
    }
}