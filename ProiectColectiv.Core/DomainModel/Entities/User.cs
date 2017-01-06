using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class User : IdentityUser
    {
        public int IdUserGroup { get; set; }

        public UserGroup UserGroup { get; set; }

        public IList<Document> Documents { get; set; } = new List<Document>();

        public IList<DocumentTask> DocumentTasks { get; set; } = new List<DocumentTask>();
    }
}
