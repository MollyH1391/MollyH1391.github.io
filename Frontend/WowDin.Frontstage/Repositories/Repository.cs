using Microsoft.EntityFrameworkCore;
using System.Linq;
using WowDin.Frontstage.Models.Entity;
using WowDin.Frontstage.Repositories.Interface;

namespace WowDin.Frontstage.Repositories
{
    public class Repository : IRepository
    {
        private readonly WowdinDbContext _context;

        public Repository(WowdinDbContext context)
        {
            _context = context;
        }

        public void Create<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Added;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Entry<T>(entity).State = EntityState.Modified;
        }
        WowdinDbContext IRepository._context { get { return _context; } }
    }
}
