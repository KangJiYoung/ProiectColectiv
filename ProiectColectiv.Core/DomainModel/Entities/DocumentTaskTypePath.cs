using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTaskTypePath
    {
        [Key]
        public int IdDocumentTaskTypePath { get; set; }

        public int IdDocumentTaskType { get; set; }

        public int IdUserGroup { get; set; }

        [ForeignKey(nameof(NextPath))]
        public int? IdNextPath { get; set; }

        public int Index { get; set; }

        public UserGroup UserGroup { get; set; }

        public DocumentTaskTypePath NextPath { get; set; }

        public DocumentTaskType DocumentTaskType { get; set; }
    }
}