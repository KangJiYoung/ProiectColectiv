using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTemplateItem
    {
        [Key]
        public int IdDocumentTemplateItem { get; set; }

        public int IdDocumentTemplate { get; set; }

        [StringLength(50)]
        public string Label { get; set; }

        public DocumentTemplate DocumentTemplate { get; set; }

        public IList<DocumentTemplateItemValue> DocumentTemplateItemValues { get; set; } = new List<DocumentTemplateItemValue>();
    }
}