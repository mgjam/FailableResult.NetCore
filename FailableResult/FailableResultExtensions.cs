using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FailableResult
{
    [DebuggerStepThrough]
    public static class FailableResultExtensions
    {
        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> result,
            Func<TResult, Task<TNewResult>> onSuccess,
            bool configureAwait = false
        ) =>
            result.Handle(
                async s => SuccessResult<TNewResult, TFailure>.Create(await onSuccess(s).ConfigureAwait(configureAwait)),
                FailureResult<TNewResult, TFailure>.CreateAsync);

        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> result,
            Func<TResult, Task<IFailableResult<TNewResult, TFailure>>> onSuccessAsync
        ) =>
            result.Handle(
                onSuccessAsync,
                f => FailureResult<TNewResult, TFailure>.CreateAsync(f));

        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, Task<TNewResult>> onSuccess,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                async s => SuccessResult<TNewResult, TFailure>.Create(await onSuccess(s).ConfigureAwait(configureAwait)),
                FailureResult<TNewResult, TFailure>.CreateAsync,
                configureAwait);

        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, TNewResult> onSuccess,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                s => SuccessResult<TNewResult, TFailure>.Create(onSuccess(s)),
                f => FailureResult<TNewResult, TFailure>.Create(f),
                configureAwait);

        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, Task<IFailableResult<TNewResult, TFailure>>> onSuccessAsync,
            bool configureAwait = false
        ) => 
            asyncResult.HandleAsync(
                onSuccessAsync,
                f => FailureResult<TNewResult, TFailure>.CreateAsync(f),
                configureAwait);

        public static Task<IFailableResult<TNewResult, TFailure>> OnSuccessAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, IFailableResult<TNewResult, TFailure>> onSuccess,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                onSuccess,
                FailureResult<TNewResult, TFailure>.Create,
                configureAwait);

        public static IFailableResult<TNewResult, TFailure> OnSuccess<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> result,
            Func<TResult, TNewResult> onSuccess
        ) =>
            result.Handle(
                s => SuccessResult<TNewResult, TFailure>.Create(onSuccess(s)),
                f => FailureResult<TNewResult, TFailure>.Create(f));

        public static IFailableResult<TNewResult, TFailure> OnSuccess<TResult, TFailure, TNewResult>(
            this IFailableResult<TResult, TFailure> result,
            Func<TResult, IFailableResult<TNewResult, TFailure>> onSuccess
        ) =>
            result.Handle(
                s => onSuccess(s),
                f => FailureResult<TNewResult, TFailure>.Create(f));

        public static Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> result,
            Func<TFailure, Task<TNewFailure>> onFailureAsync,
            bool configureAwait = false
        ) =>
            result.Handle(
                s => SuccessResult<TResult, TNewFailure>.CreateAsync(s),
                async f => FailureResult<TResult, TNewFailure>.Create(await onFailureAsync(f).ConfigureAwait(configureAwait)));

        public static Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> result,
            Func<TFailure, Task<IFailableResult<TResult, TNewFailure>>> onFailureAsync
        ) =>
            result.Handle(
                s => SuccessResult<TResult, TNewFailure>.CreateAsync(s),
                f => onFailureAsync(f));

        public static Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TFailure, Task<TNewFailure>> onFailureAsync,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                s => SuccessResult<TResult, TNewFailure>.CreateAsync(s),
                async f => FailureResult<TResult, TNewFailure>.Create(await onFailureAsync(f).ConfigureAwait(configureAwait)),
                configureAwait);

        public static Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TFailure, TNewFailure> onFailure,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                s => SuccessResult<TResult, TNewFailure>.Create(s),
                f => FailureResult<TResult, TNewFailure>.Create(onFailure(f)),
                configureAwait);

        public static Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TFailure, Task<IFailableResult<TResult, TNewFailure>>> onFailure,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                s => SuccessResult<TResult, TNewFailure>.CreateAsync(s),
                f => onFailure(f),
                configureAwait);

        public static Task<IFailableResult<TResult, TNewFailure>> OnFailureAsync<TResult, TFailure, TNewFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TFailure, IFailableResult<TResult, TNewFailure>> onFailure,
            bool configureAwait = false
        ) =>
            asyncResult.HandleAsync(
                s => SuccessResult<TResult, TNewFailure>.Create(s),
                f => onFailure(f),
                configureAwait);

        public static IFailableResult<TResult, TNewFailure> OnFailure<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> result,
            Func<TFailure, TNewFailure> onFailure
        ) =>
            result.Handle(
                s => SuccessResult<TResult, TNewFailure>.Create(s),
                f => FailureResult<TResult, TNewFailure>.Create(onFailure(f)));

        public static IFailableResult<TResult, TNewFailure> OnFailure<TResult, TFailure, TNewFailure>(
            this IFailableResult<TResult, TFailure> result,
            Func<TFailure, IFailableResult<TResult, TNewFailure>> onFailure
        ) =>
            result.Handle(
                s => SuccessResult<TResult, TNewFailure>.Create(s),
                f => onFailure(f));

        public static async Task<TResult> GetResultOrThrowExceptionAsync<TResult, TFailure>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TFailure, Exception> onFailure,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return result.GetResultOrThrowException(onFailure);
        }

        public static TResult GetResultOrThrowException<TResult, TFailure>(
            this IFailableResult<TResult, TFailure> result,
            Func<TFailure, Exception> onFailure
        ) =>
            result.Handle(
                s => s,
                f => throw onFailure(f));

        public static async Task<TNewResult> HandleAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, TNewResult> onSuccess,
            Func<TFailure, TNewResult> onFailure,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return result.Handle(onSuccess, onFailure);
        }

        public static async Task<TNewResult> HandleAsync<TResult, TFailure, TNewResult>(
            this Task<IFailableResult<TResult, TFailure>> asyncResult,
            Func<TResult, Task<TNewResult>> onSuccessAsync,
            Func<TFailure, Task<TNewResult>> onFailureAsync,
            bool configureAwait = false)
        {
            var result = await asyncResult.ConfigureAwait(configureAwait);

            return await result.Handle(onSuccessAsync, onFailureAsync).ConfigureAwait(configureAwait);
        }
    }
}
