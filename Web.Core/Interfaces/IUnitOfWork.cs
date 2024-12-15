using Web.Core.Entities;

namespace Web.Core.Interfaces
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        public Task<int> CompleteAsync();
    }
}
