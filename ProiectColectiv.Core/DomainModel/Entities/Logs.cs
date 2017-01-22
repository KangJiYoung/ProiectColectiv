using System;
using System.ComponentModel.DataAnnotations;

namespace ProiectColectiv.Core.DomainModel.Entities
{
    public class Logs
    {
        [Key]
        public int IdLog { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        [StringLength(500)]
        public string Message { get; set; }

        public User User { get; set; }
    }
}