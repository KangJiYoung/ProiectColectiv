using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentAndTasksViewModel
    {
        public List<DocumentDetailViewModel> Documents { get; set; }
        public List<DocumentTaskViewModel> Tasks { get; set; }
    }
}
