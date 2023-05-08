using System.Collections.Generic;
using System.Linq;
using WowDin.Frontstage.Models.Entity;

namespace WowDin.Frontstage.Repositories.Interface
{
    public interface IRepository
    {
        public WowdinDbContext _context { get; }
        public void Create<T>(T entity) where T : class;
        public void Update<T>(T entity) where T : class;
        public void Delete<T>(T entity) where T : class;
        public IQueryable<T> GetAll<T>() where T : class;
        public void Save();
    }
}
