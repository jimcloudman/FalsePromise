﻿using FalsePromise.Router.Models;
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
        public async Task ThrowsExceptionWithBadRequest()
        {
            var router = new RequestRouter();
            var service = new TestService();
            router.Register(service);

            Func<Task> act = async () =>
            {
                await router.Execute("pbbbbbbbbbbbt");
            };

            Assert.ThrowsAsync<RouterException>(() => act());
        }
        [Test]
        public async Task DoesNotThrowWithGoodRequest()
        {
            var router = new RequestRouter();
            var service = new TestService();
            router.Register(service);

            Func<Task> act = async () =>
            {
                await router.Execute($@"{{ ""route"": ""TestService.TestStringMethod"", ""parameters"": ""{{}}""}}");
            };

            Assert.DoesNotThrowAsync(() => act());
        }
        [Test]
        public async Task GoodRequestReturnsValue()
        {
            var router = new RequestRouter();
            var service = new TestService();
            router.Register(service);

            var result = await router.Execute($@"{{ ""route"": ""TestService.TestStringMethod"", ""parameters"": ""{{}}""}}");

            Assert.AreEqual(@"""Success""", result);
        }
        [Test]
        public async Task GoodAsyncRequestReturnsValue()
        {
            var router = new RequestRouter();
            var service = new TestAsyncService();
            router.Register(service);

            var result = await router.Execute($@"{{ ""route"": ""TestAsyncService.TestStringMethod"", ""parameters"": ""{{}}""}}");

            Assert.AreEqual(@"""Success""", result);
        }
        [Test]
        public async Task GoodComplexRequestReturnsValue()
        {
            var router = new RequestRouter();
            var service = new TestComplexService();
            router.Register(service);

            var parameterStr = "{\"parameters\":\"{\\\"SubItem\\\":null,\\\"Sample\\\":\\\"test\\\"}\",\"route\":\"TestComplexService.TestComplexParameterMethod\"}";
            var result = await router.Execute(parameterStr);

            Assert.AreEqual(@"""test""", result);
        }

        [Test]
        public async Task GoodComplexAsyncRequestReturnsValue()
        {
            var router = new RequestRouter();
            var service = new TestAsyncComplexService();
            router.Register(service);

            var parameterStr = "{\"parameters\":\"{\\\"SubItem\\\":null,\\\"Sample\\\":\\\"test\\\"}\",\"route\":\"TestAsyncComplexService.TestComplexParameterMethod\"}";
            var result = await router.Execute(parameterStr);

            Assert.AreEqual(@"""test""", result);
        }
    }
}
