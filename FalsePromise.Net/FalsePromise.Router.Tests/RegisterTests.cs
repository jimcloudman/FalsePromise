using FalsePromise.Router.Models;
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
            Action act = () =>
            {
                var router = new RequestRouter();
            };

            Assert.DoesNotThrow(() => act());
        }

        [Test]
        public void RegisterWithoutExceptions()
        {
            Action act = () =>
            {
                var router = new RequestRouter();
                var service = new TestService();
                router.Register(service);
            };

            Assert.DoesNotThrow(() => act());
        }

        [Test]
        public void DuplicateServiceTriggersException()
        {
            Action act = () =>
            {
                var router = new RequestRouter();
                var service = new TestService();
                router.Register(service);
                var serviceTwo = new TestService();
                router.Register(serviceTwo);                
            };

            Assert.Throws<RouterException>(() => act());
        }

        [Test]
        public void ServiceHasCorrectRoute()
        {
            var router = new RequestRouter();

            router.Register(new TestService());

            Assert.IsTrue(router._serviceCollection.ContainsKey("TestService.TestStringMethod"));
        }

        [Test]
        public void ServiceWithDuplicateMethodsThrowsException()
        {
            Action act = () =>
            {
                var router = new RequestRouter();
                router.Register(new TestServiceWithOverloads());
            };

            Assert.Throws<RouterException>(() => act());
        }
    }
}