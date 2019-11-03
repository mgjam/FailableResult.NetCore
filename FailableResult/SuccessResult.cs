using System;

namespace FailableResult
{
    public class SuccessResult<TResult, TFailure> : IFailableResult<TResult, TFailure>
    {
        public static IFailableResult<TResult, TFailure> Create(TResult result) =>
            new SuccessResult<TResult, TFailure>(result);

        public TResult Result { get; }

        private SuccessResult(TResult result)
        {
            Result = result;
        }

        public T Handle<T>(Func<TResult, T> onSuccess, Func<TFailure, T> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));

            return onSuccess(Result);
        }
    }
}
