using System;

namespace TestAdapterCore
{
    /// <summary>Use this attribute to mark a method as a test method.</summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class TestMethodAttribute : Attribute
    {
        /// <summary>Use this attribute to mark a method as a test method.</summary>
        public TestMethodAttribute()
        {
        }

        /// <summary>Use this attribute to mark a method as a test method.</summary>
        /// <param name="displayName">The display name for this test.</param>
        public TestMethodAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>The display name for this test.</summary>
        public string DisplayName { get; set; }
    }
}
