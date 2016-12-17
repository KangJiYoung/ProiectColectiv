using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Web.ViewModel.Mapping
{
    public static class ViewModelMapping
    {
        public static UserViewModel ConvertToViewModel(User user, UserManager<User> userManager)
            => new UserViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName,
                Role = userManager.GetRolesAsync(user).Result.FirstOrDefault()
            };

        public static IList<UserViewModel> ConvertToViewModel(IList<User> users, UserManager<User> userManager)
            => users.Select(it => ConvertToViewModel(it, userManager)).ToList();

        public static IList<DocumentDetailViewModel> ConvertToViewModel(IList<Document> documents)
            => documents.Select(ConvertToDetailViewModel).ToList();

        public static DocumentDetailViewModel ConvertToDetailViewModel(Document document)
            => new DocumentDetailViewModel
            {
                IdDocument = document.IdDocument,
                Name = document.Name,
                DateAdded = document.DateAdded,
                LastModified = document.LastModified,
                Tags = document.DocumentTags.Select(it => it.Tag.Name),
                DocumentStatus = document.DocumentStates.Last().DocumentStatus,
                CurrentVersion = document.DocumentStates.Last().Version
            };

        public static DocumentTemplateItemViewModel ConvertToViewModel(DocumentTemplateItem item)
            => new DocumentTemplateItemViewModel
            {
                IdDocumentTemplateItem = item.IdDocumentTemplateItem,
                Label = item.Label,
                DocumentTemplateItemValues = ConvertToViewModel(item.DocumentTemplateItemValues)
            };

        public static IList<DocumentTemplateItemViewModel> ConvertToViewModel(IList<DocumentTemplateItem> items)
            => items.Select(ConvertToViewModel).ToList();

        public static DocumentTemplateItemValueViewModel ConvertToViewModel(DocumentTemplateItemValue item)
            => new DocumentTemplateItemValueViewModel
            {
                Value = item.Value
            };

        public static IList<DocumentTemplateItemValueViewModel> ConvertToViewModel(IList<DocumentTemplateItemValue> items)
            => items.Select(ConvertToViewModel).ToList();
    }
}
