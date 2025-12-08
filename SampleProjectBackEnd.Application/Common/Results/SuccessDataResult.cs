namespace SampleProjectBackEnd.Application.Common.Results
{
    public class SuccessDataResult<T> : DataResult<T>
    {
        public SuccessDataResult(T data, string message = "İşlem başarılı.")
            : base(data, true, message) { }
    }
}
