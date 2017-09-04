﻿using System;

namespace FailableResult.NetCore
{
    public class FailureResult<TResult, TFailure> : IFailableResult<TResult, TFailure>
    {
        public static IFailableResult<TResult, TFailure> Create(TFailure failure)
        {
            return new FailureResult<TResult, TFailure>(failure);
        }

        public TFailure Failure { get; }

        public FailureResult(TFailure failure)
        {
            Failure = failure;
        }

        public T Handle<T>(Func<TResult, T> onSuccess, Func<TFailure, T> onFailure)
        {
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return onFailure(Failure);
        }
    }
}