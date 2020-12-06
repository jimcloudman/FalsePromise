using FalsePromise.Router.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FalsePromise.Router.Tests.TestServices
{
    internal class TestService
    {
        [Route]
        public string TestStringMethod()
        {
            return "Success";
        }
    }
    internal class TestServiceWithOverloads
    {
        [Route]
        public string TestStringMethod()
        {
            return "Success";
        }

        [Route]
        public string TestStringMethod(string problem)
        {
            return problem;
        }
    }

    internal class TestServiceWithHiddenMethods
    {
        [Route]
        public string TestStringMethod()
        {
            return "Success";
        }

        public string TestUnattributedMethod()
        {
            return "Success";
        }
    }

    internal class TestServiceWithVoidMethod
    {
        [Route]
        public void TestVoidMethod()
        {

        }

    }
    internal class TestAsyncService
    {
        [Route]
        public async Task<string> TestStringMethod()
        {
            await Task.Delay(100);
            return "Success";
        }
    }
}
