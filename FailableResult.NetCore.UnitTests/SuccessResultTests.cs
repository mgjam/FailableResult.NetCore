using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FailableResult.NetCore.UnitTests
{
    [TestClass]
    public class SuccessResultTests
    {
        [TestMethod]
        public void Should_call_correct_handle()
        {
            var result = SuccessResult<int, int>.Create(1);
            var actual = result.Handle(x => x + 1, null);
            var expected = 2;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Should_throw_When_handle_not_provided()
        {
            var result = SuccessResult<int, int>.Create(1);

            Assert.ThrowsException<ArgumentNullException>(() => result.Handle(null, x => x + 2));
        }
    }
}
