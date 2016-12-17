using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTemplate
    {
        [Key]
        public int IdDocumentTemplate { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public byte[] Data { get; set; }

        public IList<DocumentTemplateItem> DocumentTemplateItems { get; set; } = new List<DocumentTemplateItem>();
    }
}