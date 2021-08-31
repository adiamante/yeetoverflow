using System;

namespace YeetOverFlow.Core.Application.Data.Core
{
    //https://achraf-chennan.medium.com/using-the-result-class-in-c-519da90351f0
    public class Result : IResult
    {
        protected Result(Exception ex, String error)
        {
            Exception = ex;
            Success = false;
            Error = error;
        }

        protected Result(bool success, string error)
        {
            if (success && error != string.Empty)
                throw new InvalidOperationException();
            if (!success && error == string.Empty)
                throw new InvalidOperationException();
            Success = success;
            Error = error;
        }

        public bool Success { get; }
        public string Error { get; }
        public bool IsFailure => !Success;
        public Exception Exception { get; }
        public static Result Fail(string message)
        {
            return new Result(false, message);
        }
        public static Result Fail(Exception ex)
        {
            return new Result(ex, ex.Message);
        }

        public static Result Fail(Exception ex, string error)
        {
            return new Result(ex, error);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result<T> Fail<T>(Exception ex)
        {
            return new Result<T>(default, ex, ex.Message);
        }

        public static Result<T> Fail<T>(Exception ex, string message)
        {
            return new Result<T>(default, ex, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        protected internal Result(T value, bool success, string error)
            : base(success, error)
        {
            Value = value;
        }

        protected internal Result(T value, Exception ex, string error)
            : base(ex, error)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}
