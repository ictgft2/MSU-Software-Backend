namespace MedicalUnitSystem.Helpers
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Error { get; }
        public Exception Exception { get; }
        public T Value { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(string error, Exception ex = null)
        {
            IsSuccess = false;
            Error = error;
            Exception = ex;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(string error, Exception ex = null) => new Result<T>(error, ex);
    }
}
