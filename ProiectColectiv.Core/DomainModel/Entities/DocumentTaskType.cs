using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTaskType
    {
        [Key]
        public int IdDocumentTaskType { get; set; }

        public int IdDocumentTaskTemplate { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int DaysLimit { get; set; }

        public DocumentTaskTemplate DocumentTaskTemplate { get; set; }

        public IList<DocumentTaskTypePath> Paths { get; set; } = new List<DocumentTaskTypePath>();
    }
}