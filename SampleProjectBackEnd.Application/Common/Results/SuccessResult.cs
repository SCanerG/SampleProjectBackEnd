namespace SampleProjectBackEnd.Application.Common.Results
{
    public class SuccessResult : Result
    {
        public SuccessResult(string message = "İşlem başarılı.")
            : base(true, message) { }
    }
}
