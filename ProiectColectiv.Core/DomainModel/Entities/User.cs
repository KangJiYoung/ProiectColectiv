using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class User : IdentityUser
    {
        public IList<Document> Documents { get; set; } = new List<Document>();
    }
}
