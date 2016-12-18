using System.ComponentModel.DataAnnotations;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentStatusChangeViewModel
    {
        [Required]
        public int? IdDocument { get; set; }

        public DocumentStatus DocumentStatus { get; set; }
    }
}
