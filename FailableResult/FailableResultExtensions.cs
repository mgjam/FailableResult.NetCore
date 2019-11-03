using System;
using System.Diagnostics;

namespace FailableResult
{
    [DebuggerStepThrough]
    public static class FailableResultExtensions
    {
        public static IFailableResult<TNewResult, TFailure> OnSuccess<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> @this,
            Func<TResult, TNewResult> onSuccess
        ) =>
            @this.Handle(
                s => SuccessResult<TNewResult, TFailure>.Create(onSuccess(s)),
                f => FailureResult<TNewResult, TFailure>.Create(f));

        public static IFailableResult<TNewResult, TFailure> OnSuccess<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> @this,
            Func<TResult, IFailableResult<TNewResult, TFailure>> onSuccess
        ) =>
            @this.Handle(
                s => onSuccess(s),
                f => FailureResult<TNewResult, TFailure>.Create(f));

        public static IFailableResult<TResult, TNewFailure> OnFailure<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> @this,
            Func<TFailure, TNewFailure> onFailure
        ) =>
            @this.Handle(
                s => SuccessResult<TResult, TNewFailure>.Create(s),
                f => FailureResult<TResult, TNewFailure>.Create(onFailure(f)));

        public static IFailableResult<TResult, TNewFailure> OnFailure<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> @this,
            Func<TFailure, IFailableResult<TResult, TNewFailure>> onFailure
        ) =>
            @this.Handle(
                s => SuccessResult<TResult, TNewFailure>.Create(s),
                f => onFailure(f));

        public static TResult GetResultOrThrowException<TResult, TFailure>(
            this IFailableResult<TResult, TFailure> @this,
            Func<TFailure, Exception> onFailure
        ) =>
            @this.Handle(
                s => s,
                f => throw onFailure(f));
    }
}
