using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProiectColectiv.Core.DomainModel.Entities;
using ProiectColectiv.Core.Interfaces;
using ProiectColectiv.Services.Data.Context;

namespace ProiectColectiv.Services
{
    public class LogsService : ILogsService
    {
        private readonly ApplicationDbContext dbContext;

        public LogsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(string userId, string message)
        {
            dbContext.Logs.Add(new Logs { UserId = userId, Message = message, Date = DateTime.Now });
        }

        public Task<List<Logs>> GetAll()
        {
            return dbContext
                .Logs
                .Include(it => it.User)
                .ToListAsync();
        }

        public Task<List<Logs>> GetFiltered(string userId, DateTime? date)
        {
            var logs = dbContext
                .Logs
                .Include(it => it.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(userId))
                logs = logs.Where(it => it.UserId == userId);

            if (date.HasValue)
                logs = logs.Where(it =>
                    it.Date.Day == date.Value.Day &&
                    it.Date.Month == date.Value.Month &&
                    it.Date.Year == date.Value.Year);

            return logs
                .ToListAsync();
        }
    }
}