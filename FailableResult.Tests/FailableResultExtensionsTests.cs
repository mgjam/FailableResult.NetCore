using NUnit.Framework;
using System;

namespace FailableResult.Tests
{
    [TestFixture]
    public class FailableResultExtensionsTests
    {
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
    }
}
