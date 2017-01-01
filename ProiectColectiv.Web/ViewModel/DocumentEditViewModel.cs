using System.Collections.Generic;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentEditViewModel
    {
        public int IdDocument { get; set; }

        public IList<DocumentTemplateItemViewModel> Items { get; set; }
    }
}