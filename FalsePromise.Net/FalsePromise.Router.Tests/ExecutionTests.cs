using FalsePromise.Router.Models;
using FalsePromise.Router.Tests.TestServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalsePromise.Router.Tests
{
    public class ExecutionTests
    {
        [Test]
        public void ThrowsExceptionWithBadRequest()
        {
            var router = new RequestRouter();
            var service = new TestService();
            router.Register(service);

            Action act = () =>
            {
                router.Execute("pbbbbbbbbbbbt");
            };

            Assert.Throws<RouterException>(() => act());
        }
        [Test]
        public void DoesNotThrowWithGoodRequest()
        {
            var router = new RequestRouter();
            var service = new TestService();
            router.Register(service);

            Action act = () =>
            {
                router.Execute($@"{{ ""route"": ""TestService.TestStringMethod"", ""parameters"": ""{{}}""}}");
            };

            Assert.DoesNotThrow(() => act());
        }
        [Test]
        public void GoodRequestReturnsValue()
        {
            var router = new RequestRouter();
            var service = new TestService();
            router.Register(service);

            var result = router.Execute($@"{{ ""route"": ""TestService.TestStringMethod"", ""parameters"": ""{{}}""}}");

            Assert.AreEqual(@"""Success""", result);
        }
        //[Test]
        //public async Task GoodAsyncRequestReturnsValue()
        //{
        //    var router = new RequestRouter();
        //    var service = new TestAsyncService();
        //    router.Register(service);

        //    var result = await router.Execute($@"{{ ""route"": ""TestAsyncService.TestStringMethod"", ""parameters"": ""{{}}""}}");

        //    Assert.AreEqual(@"""Success""", result);
        //}
    }
}
