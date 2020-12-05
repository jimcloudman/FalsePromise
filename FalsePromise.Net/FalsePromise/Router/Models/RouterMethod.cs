using System;
using System.Reflection;

namespace FalsePromise.Router.Models
{
    internal class RouterMethod
    {
        public MethodInfo MethodInfo { get; set; }
        public Type ParameterType { get; set; }
    }
}
