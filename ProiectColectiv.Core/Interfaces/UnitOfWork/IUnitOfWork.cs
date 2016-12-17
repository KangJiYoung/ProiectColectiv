using System.Threading.Tasks;

namespace ProiectColectiv.Core.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Services

        IDocumentsService DocumentsService { get; }

        IDocumentsTemplateItemService DocumentsTemplateItemService { get; }

        IDocumentsTemplateService DocumentsTemplateService { get; }

        IRolesService RolesService { get; }

        ITagsService TagsService { get; }

        IUsersService UsersService { get; }

        #endregion

        Task<int> Commit();
    }
}