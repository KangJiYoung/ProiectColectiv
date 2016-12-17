using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTemplateStateItem
    {
        [Key]
        public int IdDocumentTemplateStateItem { get; set; }

        public int IdDocumentTemplateItem { get; set; }

        public string Value { get; set; }

        public DocumentTemplateItem DocumentTemplateItem { get; set; }
    }
}