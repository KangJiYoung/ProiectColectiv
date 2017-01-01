using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentData
    {
        [Key]
        public int IdDocumentData { get; set; }
    }

    public class DocumentDataUpload : DocumentData
    {
        public byte[] Data { get; set; }
    }

    public class DocumentDataTemplate : DocumentData
    {
        public IList<DocumentDataTemplateItem> DocumentDataTemplateItems { get; set; } = new List<DocumentDataTemplateItem>();
    }
}