using System.ComponentModel.DataAnnotations;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTaskStatusChangeViewModel
    {
        public int IdDocumentTask { get; set; }

        [Display(Name = "Document Status")]
        public DocumentTaskStatus DocumentStatus { get; set; }
    }
}