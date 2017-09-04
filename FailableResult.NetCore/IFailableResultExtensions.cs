using System;
using System.Threading.Tasks;

namespace FailableResult.NetCore
{
    // ReSharper disable once InconsistentNaming
    public static class IFailableResultExtensions
    {
        public static IFailableResult<TNewResult, TFailure> OnSuccess<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> result, Func<TResult, IFailableResult<TNewResult, TFailure>> onSuccess)
        {
            return result.Handle(
                onSuccess,
                FailureResult<TNewResult, TFailure>.Create);
        }

        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> result,
            Func<TResult, Task<IFailableResult<TNewResult, TFailure>>> onSuccessAsync)
        {
            return result.Handle(
                onSuccessAsync,
                f => Task.FromResult(FailureResult<TNewResult, TFailure>.Create(f)));
        }

        public static async Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, Task<IFailableResult<TNewResult, TFailure>>> onSuccessAsync,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);
            var newResult = await result.Handle(
                    onSuccessAsync,
                    f => Task.FromResult(FailureResult<TNewResult, TFailure>.Create(f)))
                .ConfigureAwait(configureAwait);

            return newResult;
        }

        public static async Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, IFailableResult<TNewResult, TFailure>> onSuccess,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return result.Handle(
                onSuccess,
                FailureResult<TNewResult, TFailure>.Create);
        }

        public static IFailableResult<TResult, TNewFailure> OnFailure<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> result, Func<TFailure, TNewFailure> onFailure)
        {
            return result.Handle(
                SuccessResult<TResult, TNewFailure>.Create,
                f => FailureResult<TResult, TNewFailure>.Create(onFailure(f)));
        }

        public static async Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult, Func<TFailure, TNewFailure> onFailure,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return result.Handle(
                SuccessResult<TResult, TNewFailure>.Create,
                f => FailureResult<TResult, TNewFailure>.Create(onFailure(f)));
        }

        public static IFailableResult<TResult, TFailure> OnFailure<TResult, TFailure>(
            this IFailableResult<TResult, TFailure> result, Action<TFailure> onFailure)
        {
            result.Handle(
                s => true,
                f =>
                {
                    onFailure(f);
                    return false;
                });

            return result;
        }

        public static async Task<IFailableResult<TResult, TFailure>> OnFailureAsync<TResult, TFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult, Action<TFailure> onFailure,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return result.OnFailure(onFailure);
        }

        public static TResult GetResultOrThrowException<TResult, TFailure>(
            this IFailableResult<TResult, TFailure> result,
            Func<TFailure, Exception> onFailure)
        {
            return result.Handle(
                s => s,
                f =>
                {
                    // Compilation error occurs when providing lambda expression
                    // ReSharper disable once ConvertToLambdaExpression
                    throw onFailure(f);
                });
        }

        public static async Task<TResult> GetResultOrThrowExceptionAsync<TResult, TFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TFailure, Exception> onFailure,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return result.GetResultOrThrowException(onFailure);
        }
    }
}
