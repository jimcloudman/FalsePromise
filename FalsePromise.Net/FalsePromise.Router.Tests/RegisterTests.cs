using FalsePromise.Router.Tests.TestServices;
using NUnit.Framework;
using System;

namespace FalsePromise.Router.Tests
{
    public class RegisterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConstructWithoutExceptions()
        {
            // Arrange

            // Act
            Action act = () =>
            {
                var router = new RequestRouter();
            };

            // Assert

            Assert.DoesNotThrow(() => act());
        }
    }
}