namespace MedicalUnitSystem.Helpers
{
    public class Result<T>
    {
        public string Error { get; }
        public T Value { get; }
        public bool IsSuccess => Error == null;

        private Result(T value)
        {
            Value = value;
            Error = null;
        }

        private Result(string error)
        {
            Error = error;
            Value = default;
        }

        private Result(string error, T value)
        {
            Error = error;
            Value = value;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static Result<T> Failure(string error) => new Result<T>(error);

        public static Result<T> Failure(string error, T value) => new Result<T>(error, value);
    }

}
