using ProiectColectiv.Core.DomainModel.Enums;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTaskViewModel
    {
        public int IdDocumentTask { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DocumentTaskStatus DocumentStatus { get; set; }
    }
}