using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTag
    {
        [Key]
        public int IdDocumentTag { get; set; }

        public int IdTag { get; set; }

        public int IdDocument { get; set; }

        public Tag Tag { get; set; }

        public Document Document { get; set; }
    }
}