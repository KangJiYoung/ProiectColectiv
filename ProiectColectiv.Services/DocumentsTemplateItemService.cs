using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.DomainModel.Enums;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentsTemplateItemService : IDocumentsTemplateItemService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentsTemplateItemService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private static string GetCamelCase(string word)
        {
            const string strRegex = @"(?<=[a-z])([A-Z])|(?<=[A-Z])([A-Z][a-z])";
            const string strReplace = @" $1$2";

            return new Regex(strRegex, RegexOptions.None).Replace(word, strReplace);
        }

        public Task<List<DocumentTemplateItem>> GetItemsFromTemplate(int idTemplate)
        {
            return dbContext
                .DocumentTemplateItems
                .Include(it => it.DocumentTemplateItemValues)
                .Where(it => it.IdDocumentTemplate == idTemplate)
                .ToListAsync();
        }

        public IEnumerable<DocumentTemplateItem> ParseItems(AcroFields acroFields)
        {
            foreach (var fieldKey in acroFields.Fields.Keys)
            {
                var item = new DocumentTemplateItem { Label = GetCamelCase(fieldKey) };

                if (acroFields.GetFieldType(fieldKey) == AcroFields.FIELD_TYPE_COMBO)
                    foreach (var itemValue in acroFields.GetAppearanceStates(fieldKey))
                        item.DocumentTemplateItemValues.Add(new DocumentTemplateItemValue { Value = itemValue });

                yield return item;
            }
        }
    }
}
