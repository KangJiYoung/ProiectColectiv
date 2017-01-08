using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTask
    {
        [Key]
        public int IdDocumentTask { get; set; }

        [Required]
        public string UserId { get; set; }

        public int IdDocumentTaskType { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime LastModified { get; set; }

        public DocumentTaskType DocumentTaskType { get; set; }

        public User User { get; set; }

        public IList<Document> Documents { get; set; } = new List<Document>();

        public IList<DocumentTaskState> DocumentTaskStates { get; set; } = new List<DocumentTaskState>();
    }
}