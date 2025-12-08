namespace SampleProjectBackEnd.Application.Common.Results
{
    public class ErrorResult : Result
    {
        public ErrorResult(string message = "Bir hata oluştu.")
            : base(false, message) { }
    }
}
