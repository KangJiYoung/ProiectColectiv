﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class DocumentTaskTemplatesService : IDocumentTaskTemplatesService
    {
        private readonly ApplicationDbContext dbContext;

        public DocumentTaskTemplatesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<List<DocumentTaskTemplate>> GetAll()
        {
            return dbContext
                .DocumentTaskTemplates
                .ToListAsync();
        }

        public void Add(string name, int idDocumentTemplate, IDictionary<Tuple<string, int>, IList<int>> paths)
        {
            var template = new DocumentTaskTemplate { Name = name, IdDocumentTemplate = idDocumentTemplate };

            foreach (var path in paths)
            {
                var taskType = new DocumentTaskType { Name = path.Key.Item1, DaysLimit = path.Key.Item2};
                for (var i = path.Value.Count - 1; i >= 0; i--)
                {
                    taskType.Paths.Add(new DocumentTaskTypePath
                    {
                        Index = i,
                        IdUserGroup = path.Value[i],
                        NextPath = taskType.Paths.Any() ? taskType.Paths.ElementAt(path.Value.Count - i - 2) : null
                    });
                }
                template.DocumentTaskTypes.Add(taskType);
            }

            dbContext.DocumentTaskTemplates.Add(template);
        }

        public Task<List<DocumentTaskType>> GetAllTaskTypes(int idDocumentTaskTemplate)
        {
            return dbContext
                .DocumentTaskTypes
                .Where(it => it.IdDocumentTaskTemplate == idDocumentTaskTemplate)
                .ToListAsync();
        }

        public Task<DocumentTaskTemplate> GetById(int idDocumentTaskTemplate)
        {
            return dbContext
                .DocumentTaskTemplates
                .FirstOrDefaultAsync(it => it.IdDocumentTaskTemplate == idDocumentTaskTemplate);
        }
    }
}