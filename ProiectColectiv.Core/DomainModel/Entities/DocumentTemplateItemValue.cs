using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTemplateItemValue
    {
        [Key]
        public int IdDocumentTemplateItemValue { get; set; }

        public int IdDocumentTemplateItem { get; set; }

        [Required]
        [StringLength(100)]
        public string Value { get; set; }

        public DocumentTemplateItem DocumentTemplateItem { get; set; }
    }
}