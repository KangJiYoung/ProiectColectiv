using System;
using System.Collections.Generic;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentDetailViewModel
    {
        public string Name { get; set; }

        public double CurrentVersion { get; set; }

        public DocumentStatus DocumentStatus { get; set; }

        public DateTime DateAdded { get; set; }

        public DateTime? LastModified { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}
