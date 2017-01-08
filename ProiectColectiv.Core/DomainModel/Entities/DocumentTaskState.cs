using System;
using System.ComponentModel.DataAnnotations;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTaskState
    {
        [Key]
        public int IdDocumentTaskState { get; set; }

        public int IdDocumentTask { get; set; }

        public int? IdDocumentTaskTypePath { get; set; }

        public DateTime StateDate { get; set; }

        public DocumentTaskStatus DocumentTaskStatus { get; set; }

        public DocumentTask DocumentTask { get; set; }

        public DocumentTaskTypePath DocumentTaskTypePath { get; set; }
    }
}