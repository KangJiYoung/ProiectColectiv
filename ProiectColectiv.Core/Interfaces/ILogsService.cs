using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProiectColectiv.Core.DomainModel.Entities;

namespace ProiectColectiv.Core.Interfaces
{
    public interface ILogsService
    {
        void Add(string userId, string message);
        Task<List<Logs>> GetAll();
        Task<List<Logs>> GetFiltered(string userId, DateTime? date);
    }
}