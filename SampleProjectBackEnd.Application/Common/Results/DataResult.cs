namespace SampleProjectBackEnd.Application.Common.Results
{
    public class DataResult<T> : Result, IDataResult<T>
    {
        public T Data { get; }

        protected DataResult(T data, bool success, string message)
            : base(success, message)
        {
            Data = data;
        }
    }
}
