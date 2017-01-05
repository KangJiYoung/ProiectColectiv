using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTaskTemplateAddViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Nume")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Template Document")]
        public int? IdDocumentTemplate { get; set; }

        [Display(Name = "Tipuri Task")]
        public IList<DocumentTaskTemplateTypeViewModel> Types { get; set; }
    }

    public class DocumentTaskTemplateTypeViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public IList<int> Paths { get; set; }
    }
}