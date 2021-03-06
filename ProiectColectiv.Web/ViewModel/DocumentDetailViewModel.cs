﻿using System;
using System.Collections.Generic;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentDetailViewModel
    {
        public int IdDocument { get; set; }

        public int? IdDocumentTask { get; set; }

        public bool IsFromTemplate { get; set; }

        public string Name { get; set; }

        public double CurrentVersion { get; set; }

        public bool IsDigitallySigned { get; set; }

        public DocumentStatus DocumentStatus { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? LastModified { get; set; }

        public string CreatedBy { get; set; }

        public string Abstract { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
