using System.Collections.Generic;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentTemplateItemViewModel
    {
        public int IdDocumentTemplateItem { get; set; }

        public string Label { get; set; }

        public string Value { get; set; }

        public IList<DocumentTemplateItemValueViewModel> DocumentTemplateItemValues { get; set; } = new List<DocumentTemplateItemValueViewModel>();
    }

    public class DocumentTemplateItemValueViewModel
    {
        public string Value { get; set; }
    }
}