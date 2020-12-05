using NUnit.Framework;

namespace FalsePromise.Router.Tests
{
    public class RegisterTests
    {
        [SetUp]
        public void Setup()
        {
            var testRouter = new RequestRouter();
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}