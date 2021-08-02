using System;
using System.Reflection;

namespace TestAdapterCore
{
    /// <summary>Use this attribute to specify the test assembly.</summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class TestAssemblyAttribute : Attribute
    {
        public TestAssemblyAttribute(Type typeInAssembly)
        {
            Assembly = typeInAssembly.Assembly;
        }

        public Assembly Assembly { get; }
    }
}
