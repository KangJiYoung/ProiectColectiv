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

        public static DocumentViewModel ConvertToViewModel(Document document)
            => new DocumentViewModel
            {
                Name = document.Name,
                DateAdded = document.DateAdded
            };

        public static IList<DocumentViewModel> ConvertToViewModel(IList<Document> documents)
            => documents.Select(ConvertToViewModel).ToList();
    }
}
