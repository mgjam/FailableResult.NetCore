﻿using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FailableResult.Tests
{
    [TestFixture]
    public class FailureResultTests
    {
        [Test]
        public void Should_Create_Failure_Result_When_Factory_Method_Used()
        {
            var failure = "failure";
            var failureResult = FailureResult<bool, string>.Create(failure);

            Assert.AreEqual(
                failure,
                failureResult.Handle(r => string.Empty, f => f));
            Assert.AreEqual(failure, (failureResult as FailureResult<bool, string>).Failure);
        }

        [Test]
        public async Task Should_Create_Failure_Async_Result_When_Factory_Method_Used()
        {
            var failure = "failure";
            var failureAsyncResult = FailureResult<bool, string>.CreateAsync(failure);
            var failureResult = await failureAsyncResult;
            Assert.AreEqual(failure, (failureResult as FailureResult<bool, string>).Failure);
        }

        [Test]
        public void Should_Throw_Exception_When_Handler_Not_Provided()
        {
            var failure = "failure";
            var failureResult = FailureResult<bool, string>.Create(failure);

            Assert.Throws<ArgumentNullException>(() =>
                failureResult.Handle(r => r, null));
        }
    }
}
