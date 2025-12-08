namespace SampleProjectBackEnd.Application.Common.Results
{
    public class Result : IResult
    {
        public bool Success { get; }
        public string Message { get; }

        protected Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
