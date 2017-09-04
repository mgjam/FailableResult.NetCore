using System;

namespace FailableResult.NetCore
{
    /// <summary>
    /// Specifies a result which is not available in case of failure.
    /// Result is either of type <see cref="TResult"/>, or of type <see cref="TFailure"/>
    /// </summary>
    /// <typeparam name="TResult">Type of the sucessful result</typeparam>
    /// <typeparam name="TFailure">Type of the failure</typeparam>
    public interface IFailableResult<out TResult, out TFailure>
    {
        /// <summary>
        /// Handles the failable result. Either handle for result, 
        /// or handle for failure will be triggered depending on type of the result.
        /// </summary>
        T Handle<T>(Func<TResult, T> onSuccess, Func<TFailure, T> onFailure);
    }
}
