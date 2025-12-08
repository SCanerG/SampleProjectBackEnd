namespace SampleProjectBackEnd.Application.Common.Results
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
