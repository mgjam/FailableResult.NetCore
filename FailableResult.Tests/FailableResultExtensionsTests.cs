using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FailableResult.Tests
{
    [TestFixture]
    public class FailableResultExtensionsTests
    {
        [Test]
        public async Task Should_Execute_Task_Continuation()
        {
            var successResult = SuccessResult<int, int>.Create(1);
            var failureResult = FailureResult<int, int>.Create(2);

            Assert.AreEqual(
                2,
                await successResult
                    .OnSuccessAsync(s => Task.FromResult(s * 2))
                    .OnFailureAsync(f => Task.FromResult(f * 5))
                    .GetResultOrThrowExceptionAsync(f => new Exception()));
            Assert.AreEqual(
                2,
                await successResult
                    .OnSuccessAsync(s => SuccessResult<int, int>.CreateAsync(s * 2))
                    .OnFailureAsync(f => FailureResult<int, int>.CreateAsync(f * 5))
                    .GetResultOrThrowExceptionAsync(f => new Exception()));
            Assert.AreEqual(
                4,
                await failureResult
                    .OnFailureAsync(f => Task.FromResult(f * 2))
                    .OnSuccessAsync(s => Task.FromResult(s * 5))
                    .HandleAsync(s => 0, f => f));
            Assert.AreEqual(
                4,
                await failureResult
                    .OnFailureAsync(f => FailureResult<int, int>.CreateAsync(f * 2))
                    .OnSuccessAsync(s => SuccessResult<int, int>.CreateAsync(s * 5))
                    .HandleAsync(s => 0, f => f));
        }

        [Test]
        public async Task Should_Execute_Task_Continuation_From_Task()
        {
            var asyncSuccessResult = SuccessResult<int, int>.CreateAsync(1);
            var asyncFailureResult = FailureResult<int, int>.CreateAsync(2);

            Assert.AreEqual(
                2,
                await asyncSuccessResult
                    .OnSuccessAsync(s => Task.FromResult(s * 2))
                    .OnFailureAsync(f => Task.FromResult(f * 5))
                    .GetResultOrThrowExceptionAsync(f => new Exception()));
            Assert.AreEqual(
                2,
                await asyncSuccessResult
                    .OnSuccessAsync(s => s * 2)
                    .OnFailureAsync(f => f * 5)
                    .GetResultOrThrowExceptionAsync(f => new Exception()));
            Assert.AreEqual(
                2,
                await asyncSuccessResult
                    .OnSuccessAsync(s => SuccessResult<int, int>.CreateAsync(s * 2))
                    .OnFailureAsync(f => FailureResult<int, int>.CreateAsync(f * 5))
                    .GetResultOrThrowExceptionAsync(f => new Exception()));
            Assert.AreEqual(
                2,
                await asyncSuccessResult
                    .OnSuccessAsync(s => SuccessResult<int, int>.Create(s * 2))
                    .OnFailureAsync(f => FailureResult<int, int>.Create(f * 5))
                    .GetResultOrThrowExceptionAsync(f => new Exception()));
            Assert.AreEqual(
                4,
                await asyncFailureResult
                    .OnFailureAsync(f => Task.FromResult(f * 2))
                    .OnSuccessAsync(s => Task.FromResult(s * 5))
                    .HandleAsync(s => 0, f => f));
            Assert.AreEqual(
                4,
                await asyncFailureResult
                    .OnFailureAsync(f => f * 2)
                    .OnSuccessAsync(s => s * 5)
                    .HandleAsync(s => 0, f => f));
            Assert.AreEqual(
                4,
                await asyncFailureResult
                    .OnFailureAsync(f => FailureResult<int, int>.CreateAsync(f * 2))
                    .OnSuccessAsync(s => SuccessResult<int, int>.CreateAsync(s * 5))
                    .HandleAsync(s => 0, f => f));
            Assert.AreEqual(
                4,
                await asyncFailureResult
                    .OnFailureAsync(f => FailureResult<int, int>.Create(f * 2))
                    .OnSuccessAsync(s => SuccessResult<int, int>.Create(s * 5))
                    .HandleAsync(s => 0, f => f));
        }

        [Test]
        public void Should_Get_New_Result_When_On_Success()
        {
            var result = "result";
            var failableResult = SuccessResult<string, string>.Create(result);
            var newResult = 2;
            var newFailableResult = failableResult.OnSuccess(r => newResult);

            Assert.AreEqual(newResult, newFailableResult.GetResultOrThrowException(f => new Exception()));
        }

        [Test]
        public void Should_Chain_New_Results_When_On_Success()
        {
            var result = SuccessResult<int, int>
                .Create(1)
                .OnSuccess(s => s * 2)
                .OnSuccess(s => s + 5);

            Assert.AreEqual(7, result.GetResultOrThrowException(f => new Exception()));
        }

        [Test]
        public void Should_Get_New_Result_When_New_Failable_Result_On_Success()
        {
            var result = SuccessResult<int , int>
                .Create(1)
                .OnSuccess(s => SuccessResult<int, int>.Create(s + 5));

            Assert.AreEqual(6, result.GetResultOrThrowException(f => new Exception()));
        }

        [Test]
        public void Should_Not_Get_New_Result_On_Success_Failable_Result_When_Is_Failure()
        {
            var result = FailureResult<int, int>
                .Create(1)
                .OnSuccess(s => SuccessResult<int, int>.Create(s + 5));

            Assert.AreEqual(1, result.Handle(s => s, f => f));
        }

        [Test]
        public void Should_Get_New_Failure_When_On_Failure()
        {
            var failure = FailureResult<int, int>
                .Create(2)
                .OnSuccess(s => s * 2)
                .OnFailure(f => f * 3);

            Assert.AreEqual(6, failure.Handle(s => s, f => f));
        }

        [Test]
        public void Should_Not_Get_New_Failure_On_Failure_When_Is_Success()
        {
            var success = SuccessResult<int, int>
                .Create(2)
                .OnSuccess(s => s * 2)
                .OnFailure(f => f * 3);

            Assert.AreEqual(4, success.Handle(s => s, f => f));
        }

        [Test]
        public void Should_Get_New_Failure_Failable_Result_When_On_Failure()
        {
            var failure = FailureResult<int, int>
                .Create(2)
                .OnSuccess(s => s * 2)
                .OnFailure(f => FailureResult<int, int>.Create(f * 3));

            Assert.AreEqual(6, failure.Handle(s => s, f => f));
        }

        [Test]
        public void Should_Not_Get_New_Failure_Failable_Result_On_Failure_When_Is_Success()
        {
            var success = SuccessResult<int, int>
                .Create(2)
                .OnSuccess(s => s * 2)
                .OnFailure(f => FailureResult<int, int>.Create(f * 3));

            Assert.AreEqual(4, success.Handle(s => s, f => f));
        }

        [Test]
        public void Should_Not_Get_New_Result_On_Success_When_Is_Failure()
        {
            var failure = "failure";
            var FailableResult = FailureResult<string, string>.Create(failure);
            var newResult = 2;
            var newFailableResult = FailableResult.OnSuccess(r => newResult);

            Assert.AreEqual("failure", newFailableResult.Handle(s => string.Empty, f => f));
        }

        [Test]
        public async Task Should_Get_Async_Result_When_Available()
        {
            var result = "result";
            var failableAsyncResult = SuccessResult<string, bool>.CreateAsync(result);

            Assert.AreEqual(result, await failableAsyncResult.GetResultOrThrowExceptionAsync(f => new Exception()));
        }

        [Test]
        public void Should_Throw_Exception_When_No_Async_Result()
        {
            var failure = "failure";
            var failableAsyncResult = FailureResult<string, string>.CreateAsync(failure);

            Assert.ThrowsAsync<Exception>(async () => await failableAsyncResult.GetResultOrThrowExceptionAsync(f => new Exception()));
        }

        [Test]
        public void Should_Get_Result_When_Available()
        {
            var result = "result";
            var failableResult = SuccessResult<string, bool>.Create(result);

            Assert.AreEqual(result, failableResult.GetResultOrThrowException(f => new Exception()));
        }

        [Test]
        public void Should_Throw_Exception_When_No_Result()
        {
            var failure = "failure";
            var failableResult = FailureResult<string, string>.Create(failure);

            Assert.Throws<Exception>(() => failableResult.GetResultOrThrowException(f => new Exception()));
        }

        [Test]
        public async Task Should_handle_async_result()
        {
            var asyncSuccessResult = SuccessResult<int, int>.CreateAsync(1);
            var asyncFailureResult = FailureResult<int, int>.CreateAsync(1);
            Func<int, int> f1 = x => x * 2;
            Func<int, int> f2 = x => -x;
            var handledSuccessResult = await asyncSuccessResult.HandleAsync(f1, f2);
            var handledFailureResult = await asyncFailureResult.HandleAsync(f1, f2);

            Assert.AreEqual(2, handledSuccessResult);
            Assert.AreEqual(-1, handledFailureResult);
        }

        [Test]
        public async Task Should_handle_async_result_with_async_continuations()
        {
            var asyncSuccessResult = SuccessResult<int, int>.CreateAsync(1);
            var asyncFailureResult = FailureResult<int, int>.CreateAsync(1);
            Func<int, Task<int>> f1 = x => Task.FromResult(x * 2);
            Func<int, Task<int>> f2 = x => Task.FromResult(-x);
            var handledSuccessResult = await asyncSuccessResult.HandleAsync(f1, f2);
            var handledFailureResult = await asyncFailureResult.HandleAsync(f1, f2);

            Assert.AreEqual(2, handledSuccessResult);
            Assert.AreEqual(-1, handledFailureResult);
        }
    }
}
