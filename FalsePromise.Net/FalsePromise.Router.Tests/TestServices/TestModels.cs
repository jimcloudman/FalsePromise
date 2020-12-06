using System;
using System.Collections.Generic;
using System.Text;

namespace FalsePromise.Router.Tests.TestServices
{
    public class TestComplexClass
    {
        public string Sample { get; set; }
        public TestComplexClassSubItem SubItem { get; set; }
    }

    public class TestComplexClassSubItem
    {
        public int Value { get; set; }
    }
}
