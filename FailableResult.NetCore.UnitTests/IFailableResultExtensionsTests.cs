using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FailableResult.NetCore.UnitTests
{
    [TestClass]
    // ReSharper disable once InconsistentNaming
    public class IFailableResultExtensionsUnitTests
    {
        [TestMethod]
        public void Should_transform_result_on_success()
        {
            var successResult = SuccessResult<int, int>.Create(1);
            var failureResult = FailureResult<int, int>.Create(1);
            var successTransform = successResult.OnSuccess(x => SuccessResult<int, int>.Create(x + 1));
            var failureTransform = failureResult.OnSuccess(x => SuccessResult<int, int>.Create(x + 1));
            var successActual = successTransform.Handle(x => x, x => x);
            var failureActual = failureTransform.Handle(x => x, x => x);

            Assert.AreEqual(2, successActual);
            Assert.AreEqual(1, failureActual);
        }

        [TestMethod]
        public void Should_transform_result_on_failure()
        {
            var successResult = SuccessResult<int, int>.Create(1);
            var failureResult = FailureResult<int, int>.Create(1);
            var successTransform = successResult.OnFailure(x => x + 1);
            var failureTransform = failureResult.OnFailure(x => x + 1);
            var successActual = successTransform.Handle(x => x, x => x);
            var failureActual = failureTransform.Handle(x => x, x => x);

            Assert.AreEqual(1, successActual);
            Assert.AreEqual(2, failureActual);
        }

        [TestMethod]
        public async Task Should_transform_results_async()
        {
            Func<int, IFailableResult<int, int>> f1 = x => SuccessResult<int, int>.Create(x + 1);
            Func<int, Task<IFailableResult<int, int>>> f2 = x => Task.FromResult(SuccessResult<int, int>.Create(x + 2));
            Func<int, int> f3 = x => x - 3;

            var successResult = SuccessResult<int, int>.Create(1);
            var failureResult = FailureResult<int, int>.Create(1);

            var successTransform = await successResult
                .OnSuccess(f1)
                .OnFailure(f3)
                .OnSuccessAsync(f2)
                .OnSuccessAsync(f1)
                .OnFailureAsync(f3);
            var failureTransform = await failureResult
                .OnSuccess(f1)
                .OnFailure(f3)
                .OnSuccessAsync(f2)
                .OnSuccessAsync(f1)
                .OnFailureAsync(f3);

            var successActual = successTransform.Handle(x => x, x => x);
            var failureActual = failureTransform.Handle(x => x, x => x);

            Assert.AreEqual(5, successActual);
            Assert.AreEqual(-5, failureActual);
        }

        [TestMethod]
        public void Should_execute_action_on_failure()
        {
            var successResult = SuccessResult<int, int>.Create(1);
            var failureResult = FailureResult<int, int>.Create(1);
            var successCallback = 0;
            var failureCallback = 0;

            successResult.OnFailure(i => successCallback = i);
            failureResult.OnFailure(i => failureCallback = i);

            Assert.AreEqual(0, successCallback);
            Assert.AreEqual(1, failureCallback);
        }

        [TestMethod]
        public async Task Should_execute_action_on_failure_async()
        {
            var successResult = Task.FromResult(SuccessResult<int, int>.Create(1));
            var failureResult = Task.FromResult(FailureResult<int, int>.Create(1));
            var successCallback = 0;
            var failureCallback = 0;

            await successResult.OnFailureAsync(i => successCallback = i);
            await failureResult.OnFailureAsync(i => failureCallback = i);

            Assert.AreEqual(0, successCallback);
            Assert.AreEqual(1, failureCallback);
        }

        [TestMethod]
        public void Should_throw_exception_when_result_not_available()
        {
            var successResult = SuccessResult<int, int>.Create(1);
            var failureResult = FailureResult<int, int>.Create(12345);
            Func<int, Exception> exFunc = i => new Exception(i.ToString());

            var result = successResult.GetResultOrThrowException(exFunc);
            Assert.AreEqual(1, result);

            var ex = Assert.ThrowsException<Exception>(() => failureResult.GetResultOrThrowException(exFunc));
            Assert.IsTrue(ex.ToString().Contains("12345"));
        }

        [TestMethod]
        public async Task Should_throw_exception_when_result_not_available_async()
        {
            var successResult = Task.FromResult(SuccessResult<int, int>.Create(1));
            var failureResult = Task.FromResult(FailureResult<int, int>.Create(12345));
            Func<int, Exception> exFunc = i => new Exception(i.ToString());

            var result = await successResult.GetResultOrThrowExceptionAsync(exFunc);
            Assert.AreEqual(1, result);

            var ex = await Assert.ThrowsExceptionAsync<Exception>(() => failureResult.GetResultOrThrowExceptionAsync(exFunc));
            Assert.IsTrue(ex.ToString().Contains("12345"));
        }
    }
}
