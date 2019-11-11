using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FailableResult.Tests
{
    [TestFixture]
    public class SuccessResultTests
    {
        [Test]
        public void Should_Create_Success_Result_When_Factory_Method_Used()
        {
            var result = "success";
            var successResult = SuccessResult<string, bool>.Create(result);

            Assert.AreEqual(
                result,
                successResult.Handle(r => r, f => string.Empty));
            Assert.AreEqual(result, (successResult as SuccessResult<string,bool>).Result);
        }

        [Test]
        public async Task Should_Create_Success_Async_Result_When_Factory_Method_Used()
        {
            var result = "success";
            var successAsyncResult = SuccessResult<string, bool>.CreateAsync(result);
            var successResult = await successAsyncResult;
            Assert.AreEqual(result, (successResult as SuccessResult<string, bool>).Result);
        }

        [Test]
        public void Should_Throw_Exception_When_Handler_Not_Provided()
        {
            var result = "success";
            var successResult = SuccessResult<string, bool>.Create(result);

            Assert.Throws<ArgumentNullException>(() =>
                successResult.Handle(null, f => f));
        }
    }
}
