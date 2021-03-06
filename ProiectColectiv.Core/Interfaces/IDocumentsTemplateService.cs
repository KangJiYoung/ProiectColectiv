﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface IDocumentsTemplateService
    {
        void AddTemplate(string name, byte[] data);
        Task<List<DocumentTemplate>> GetAllTemplates();
        Task<DocumentTemplate> GetTemplateById(int idTemplate);
    }
}