﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class Document
    {
        [Key]
        public int IdDocument { get; set; }

        [Required]
        public string UserId { get; set; }

        public int? IdDocumentTask { get; set; }

        public int? IdDocumentTemplate { get; set; }

        [StringLength(100)]
        public string Abstract { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? LastModified { get; set; }

        public User User { get; set; }

        public DocumentTask DocumentTask { get; set; }

        public DocumentTemplate DocumentTemplate { get; set; }

        public IList<DocumentTag> DocumentTags { get; set; } = new List<DocumentTag>();

        public IList<DocumentState> DocumentStates { get; set; } = new List<DocumentState>();
    }
}