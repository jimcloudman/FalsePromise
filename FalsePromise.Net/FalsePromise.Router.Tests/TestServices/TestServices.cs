using FalsePromise.Router.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
}
