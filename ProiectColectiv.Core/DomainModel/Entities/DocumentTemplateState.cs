using System.Collections.Generic;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class DocumentTemplateState : DocumentState
    {
        public IList<DocumentTemplateStateItem> DocumentTemplateStateItems { get; set; } = new List<DocumentTemplateStateItem>();
    }
}