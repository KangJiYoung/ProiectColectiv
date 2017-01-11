using System;
using System.Collections.Generic;
using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTaskDetailViewModel
    {
        public int IdDocumentTask { get; set; }

        public int CurrentUserGroupId { get; set; }

        public int RequireActionUserGroupId { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public string Type { get; set; }

        public DocumentTaskStatus Status { get; set; }

        public DateTime LastModified { get; set; }

        public DateTime DateAdded { get; set; }

        public string Progress { get; set; }

        public IList<DocumentDetailViewModel> Documents { get; set; }
    }
}