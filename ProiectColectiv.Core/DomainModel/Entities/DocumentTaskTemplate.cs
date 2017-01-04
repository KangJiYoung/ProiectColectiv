using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTaskTemplate
    {
        [Key]
        public int IdDocumentTaskTemplate { get; set; }

        public int IdDocumentTemplate { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DocumentTemplate DocumentTemplate { get; set; }

        public IList<DocumentTaskType> DocumentTaskTypes { get; set; }
    }
}