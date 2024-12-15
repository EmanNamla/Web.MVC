using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core.Entities;
using Web.Core.Interfaces;
using Web.Infrastructure.Data;

namespace Web.Infrastructure.Repositories
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        private readonly Hashtable _repository;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this._repository = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        { return await dbContext.SaveChangesAsync(); }

        public async ValueTask DisposeAsync()
        => await dbContext.DisposeAsync();

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repository.ContainsKey(type))
            {
                var Repository = new Repository<TEntity>(dbContext);
                _repository.Add(type, Repository);
            }
            return _repository[type] as IRepository<TEntity>;
        }
    }
}
