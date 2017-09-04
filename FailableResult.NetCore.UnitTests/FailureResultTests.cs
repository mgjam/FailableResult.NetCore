using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FailableResult.NetCore.UnitTests
{
    [TestClass]
    public class FailureResultTests
    {
        [TestMethod]
        public void Should_call_correct_handle()
        {
            var result = FailureResult<int, int>.Create(1);
            var actual = result.Handle(null, x => x + 2);
            var expected = 3;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_throw_When_handle_not_provided()
        {
            var result = FailureResult<int, int>.Create(1);

            Assert.ThrowsException<ArgumentNullException>(() => result.Handle(x => x + 1, null));
        }
    }
}
