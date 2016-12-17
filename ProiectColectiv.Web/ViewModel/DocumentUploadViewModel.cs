using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using ProiectColectiv.Web.Application.Attributes;

namespace ProiectColectiv.Web.ViewModel
{
    public class DocumentUploadViewModel
    {
        public bool IsTemplate { get; set; }

        [StringLength(100)]
        [Display(Name = "Descriere")]
        public string Abstract { get; set; }

        [Display(Name = "Tags")]
        [EnsureOneElement(ErrorMessage = "Adaugati cel putin 1 tag")]
        public IList<string> Tags { get; set; }

        #region Template

        [Display(Name = "Nume Document")]
        [RequiredIf(nameof(IsTemplate), true)]
        public string DocumentName { get; set; }

        [RequiredIf(nameof(IsTemplate), true)]
        public int? IdTemplate { get; set; }

        [RequiredIf(nameof(IsTemplate), true)]
        public IList<DocumentTemplateItemViewModel> Items { get; set; }

        #endregion

        #region Document

        [DataType(DataType.Upload)]
        [Display(Name = "Fisier Document")]
        [RequiredIf(nameof(IsTemplate), false, ErrorMessage = "Incarca un Document valid")]
        public IFormFile File { get; set; }

        #endregion
    }
}
