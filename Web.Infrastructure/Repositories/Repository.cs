using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Entities;
using Web.Core.Interfaces;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int? id)
            => await _context.Set<T>().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        

        public async Task<IEnumerable<T>> GetAllAsync() => await _context.Set<T>().ToListAsync();

        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);

        public void Update(T entity) => _context.Set<T>().Update(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public async Task<T> GetByIdAsync(int? id, params string[] includes)
        {
            if (id == null)
                return null;

            var query = _context.Set<T>().AsQueryable();

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, params string[] includes)
        {
                var collection = _context.Set<T>().Where(predicate);

                if (includes.Any())
                {
                    includes.ToList().ForEach(x => collection = collection.Include(x));
                }

                return await collection.ToListAsync();
            
        }
    }
}
