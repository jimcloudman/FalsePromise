using FalsePromise.Router.Models;
using FalsePromise.Router.Tests.TestServices;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FalsePromise.Router.Tests
{
    public class ExecutionTests
    {

        [Test]
        public void ExecuteThrowsExceptionWithBadRequest()
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
    }
}
