using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Services

        IDocumentsService DocumentsService { get; }

        IDocumentsStatesService DocumentsStatesService { get; }

        IDocumentsTemplateItemService DocumentsTemplateItemService { get; }

        IDocumentsTemplateService DocumentsTemplateService { get; }

        IDocumentTaskTemplatesService DocumentTaskTemplatesService { get; }

        IDocumentTasksService DocumentTasksService { get; set; }

        ILogsService LogsService { get; set; }

        IRolesService RolesService { get; }

        ITagsService TagsService { get; }

        IUsersService UsersService { get; }

        IUserGroups UserGroupsService { get; set; }

        #endregion

        Task<int> Commit();
    }
}