using System;
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
                Role = userManager.GetRolesAsync(user).Result.FirstOrDefault(),
                Group = user.UserGroup.Name,
                GroupId = user.IdUserGroup
            };

        public static IList<UserViewModel> ConvertToViewModel(IList<User> users, UserManager<User> userManager)
            => users.Select(it => ConvertToViewModel(it, userManager)).ToList();

        public static IList<DocumentDetailViewModel> ConvertToViewModel(IList<Document> documents)
            => documents.Select(ConvertToDetailViewModel).ToList();

        public static DocumentDetailViewModel ConvertToDetailViewModel(Document document)
            => new DocumentDetailViewModel
            {
                IdDocument = document.IdDocument,
                IsFromTemplate = document.IdDocumentTemplate != null,
                Name = document.Name,
                DateAdded = document.DateAdded,
                LastModified = document.LastModified,
                DocumentStatus = document.DocumentStates.Last().DocumentStatus,
                CurrentVersion = document.DocumentStates.Last().Version,
                Abstract = document.Abstract,
                CreatedBy = document.User?.UserName,
                Tags = document.DocumentTags.Select(it => it.Tag.Name)
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

        public static DocumentTemplateItemViewModel ConvertToViewModel(DocumentDataTemplateItem item)
            => new DocumentTemplateItemViewModel
            {
                IdDocumentTemplateItem = item.IdDocumentTemplateItem,
                Label = item.DocumentTemplateItem.Label,
                Value = item.Value,
                DocumentTemplateItemValues = ConvertToViewModel(item.DocumentTemplateItem.DocumentTemplateItemValues)
            };

        public static IList<DocumentTemplateItemViewModel> ConvertToViewModel(IList<DocumentDataTemplateItem> items)
            => items.Select(ConvertToViewModel).ToList();
    }
}
