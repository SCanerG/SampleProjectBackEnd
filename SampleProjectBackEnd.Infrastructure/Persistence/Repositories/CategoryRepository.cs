using SampleProjectBackEnd.Application.Interfaces.Repositories;
using SampleProjectBackEnd.Domain.Entities;

namespace SampleProjectBackEnd.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}