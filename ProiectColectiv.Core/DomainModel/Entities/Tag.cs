using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class Tag
    {
        [Key]
        public int IdTag { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; }

        public IList<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();
    }
}