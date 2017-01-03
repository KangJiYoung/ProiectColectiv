using System.Collections.Generic;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;
using System.Linq;

namespace ProiectColectiv.Services
{
    public class DocumentsTemplateService : IDocumentsTemplateService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentsTemplateService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddTemplate(string name, byte[] data)
        {
            using (var reader = new PdfReader(data))
            {
                var template = new DocumentTemplate
                {
                    Data = data,
                    Name = name,
                    DocumentTemplateItems = ParseItems(reader.AcroFields).ToList()
                };

                dbContext.DocumentTemplates.Add(template);
            }
        }

        private static IEnumerable<DocumentTemplateItem> ParseItems(AcroFields acroFields)
        {
            foreach (var fieldKey in acroFields.Fields.Keys)
            {
                var item = new DocumentTemplateItem { Label = Core.Utils.StringUtils.GetCamelCase(fieldKey) };

                if (acroFields.GetFieldType(fieldKey) == AcroFields.FIELD_TYPE_COMBO)
                    foreach (var itemValue in acroFields.GetAppearanceStates(fieldKey))
                        item.DocumentTemplateItemValues.Add(new DocumentTemplateItemValue { Value = itemValue });

                yield return item;
            }
        }

        public Task<List<DocumentTemplate>> GetAllTemplates()
        {
            return dbContext
                .DocumentTemplates
                .ToListAsync();
        }

        public Task<DocumentTemplate> GetTemplateById(int idTemplate)
        {
            return dbContext
                .DocumentTemplates
                .FirstAsync(it => it.IdDocumentTemplate == idTemplate);
        }
    }
}
