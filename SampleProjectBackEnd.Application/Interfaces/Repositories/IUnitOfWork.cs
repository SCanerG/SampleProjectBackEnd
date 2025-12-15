
namespace SampleProjectBackEnd.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        Task<int> CommitAsync();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
