namespace SampleProjectBackEnd.Application.Common.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(string message = "Bir hata oluştu.")
            : base(default!, false, message) { }
    }
}
