﻿using System.Threading.Tasks;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Core.Interfaces.UnitOfWork;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;

            DocumentsService = new DocumentsService(dbContext);
            DocumentsStatesService = new DocumentsStatesService(dbContext);
            DocumentsTemplateItemService = new DocumentsTemplateItemService(dbContext);
            DocumentsTemplateService = new DocumentsTemplateService(dbContext);
            RolesService = new RolesService(dbContext);
            TagsService = new TagsService(dbContext);
            UsersService = new UsersService(dbContext);
        }

        #region Services

        public IDocumentsService DocumentsService { get; }

        public IDocumentsStatesService DocumentsStatesService { get; }

        public IDocumentsTemplateItemService DocumentsTemplateItemService { get; }

        public IDocumentsTemplateService DocumentsTemplateService { get; }

        public IRolesService RolesService { get; }

        public ITagsService TagsService { get; }

        public IUsersService UsersService { get; }

        #endregion

        public Task<int> Commit() => dbContext.SaveChangesAsync();
    }
}
