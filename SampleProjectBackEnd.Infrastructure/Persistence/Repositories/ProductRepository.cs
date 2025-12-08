using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Domain.Entities;

namespace SampleProjectBackEnd.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(PersistenceContext context) : base(context)
        {
        }

        // Özel sorgular örnek:
        // public async Task<IEnumerable<Product>> GetLowStockProductsAsync()
        // {
        //     return await _dbSet.Where(p => p.Stock < 10).ToListAsync();
        // }
    }
}
