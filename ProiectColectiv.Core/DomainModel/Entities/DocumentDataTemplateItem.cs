using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentDataTemplateItem
    {
        [Key]
        public int IdDocumentDataTemplateItem { get; set; }

        public int IdDocumentTemplateItem { get; set; }

        public int IdDocumentData { get; set; }

        public string Value { get; set; }

        public DocumentData DocumentData { get; set; }

        public DocumentTemplateItem DocumentTemplateItem { get; set; }
    }
}