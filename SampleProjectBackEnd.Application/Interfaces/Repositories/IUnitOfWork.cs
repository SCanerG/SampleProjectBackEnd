namespace SampleProjectBackEnd.Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        Task<int> CommitAsync();
    }
}
