using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using Web.Application.DTOs.SetUp;
using Web.Application.Interfaces;
using Web.Core.Entities;
using Web.Core.Interfaces;

namespace Web.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCurrentUser()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        private async Task<T?> GetEntityByIdAsync<T>(int id, params string[] includes) where T : BaseEntity
        {
            var repository = _unitOfWork.Repository<T>();
            return await repository.GetByIdAsync(id, includes);
        }

        private async Task<IEnumerable<T>> GetAllEntitiesAsync<T>(Expression<Func<T, bool>> predicate, params string[] includes) where T : BaseEntity
        {
            var repository = _unitOfWork.Repository<T>();
            return await repository.GetAllAsync(predicate, includes);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string? categoryName)
        {
            Expression<Func<Product, bool>> predicate = PredicateBuilder.New<Product>(p => p.EndDate > DateTime.Now);

            var includes = new[] { nameof(Category) };
            var products = await GetAllEntitiesAsync(predicate, includes);

            if (!string.IsNullOrEmpty(categoryName))
            {
                var categories = await GetAllEntitiesAsync<Category>(c => true);
                var matchedCategory = categories
                    .FirstOrDefault(c => c.Name.Trim().ToLower().Contains(categoryName.Trim().ToLower()));

                if (matchedCategory != null)
                {
                    products = products.Where(p => p.CategoryId == matchedCategory.Id).ToList();
                }
                else
                {
                    return Enumerable.Empty<Product>();
                }
            }

            return products;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await GetEntityByIdAsync<Product>(id, nameof(Category));
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            var createdBy = GetCurrentUser();
            var repository = _unitOfWork.Repository<Product>();

            var product = dto.Adapt<Product>();
            product.CreatedBy = createdBy;

            await repository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateProductAsync(ProductDto dto)
        {
            var createdBy = GetCurrentUser();
            var repository = _unitOfWork.Repository<Product>();

            var existingProduct = await repository.GetByIdAsync(dto.Id);
            if (existingProduct == null)
                throw new Exception("Product not found");

            var updatedProduct = dto.Adapt(existingProduct);
            updatedProduct.CreatedBy = createdBy;

            repository.Update(updatedProduct);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var repository = _unitOfWork.Repository<Product>();

            var product = await repository.GetByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            repository.Delete(product);
            await _unitOfWork.CompleteAsync();
        }
    }
}
