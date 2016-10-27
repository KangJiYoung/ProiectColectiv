using System;
using System.ComponentModel.DataAnnotations;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentState
    {
        [Key]
        public int IdDocumentState { get; set; }

        public int IdDocument { get; set; }

        public DateTime StatusDate { get; set; }

        public DocumentStatus DocumentStatus { get; set; }

        public double Version { get; set; }

        public byte[] Data { get; set; }

        public Document Document { get; set; }
    }
}