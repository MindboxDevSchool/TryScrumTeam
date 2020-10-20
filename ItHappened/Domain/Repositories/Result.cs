using System;

namespace ItHappened.Domain.Repositories
{
    public class Result<T>
    {
        public Result(T value)
        {
            Value = value;
        }
        
        public T Value { get; }
        public Exception Exception { get; }
        
        public Result(Exception exception)
        {
            Exception = exception;
        }

        public bool IsSuccessful()
        {
            return Exception == null;
        }

        
    }
}