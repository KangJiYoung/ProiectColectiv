using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class UserGroup
    {
        [Key]
        public int IdUserGroup { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public IList<User> Users { get; set; }
    }
}