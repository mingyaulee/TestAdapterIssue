using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TestAdapterCore;
using TestAdapterCore.Infrastructure;

namespace VS.TestAdapter
{
    [FileExtension(".dll")]
    [DefaultExecutorUri(TestExecutor.ExecutorUri)]
    public class TestDiscoverer : ITestDiscoverer
    {
        public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
        {
            var testCases = FindTestCases(sources);
            foreach (var testCase in testCases)
            {
                discoverySink.SendTestCase(testCase);
            }
        }

        public IEnumerable<TestCase> FindTestCases(IEnumerable<string> sources)
        {
            var testCases = new List<TestCase>();
            using var testAssemblyLoader = new TestAssemblyLoader();
            foreach (var source in sources)
            {
                // Load the assembly and find the assembly attribute which specifies which referenced assembly to search for tests
                var assembly = testAssemblyLoader.LoadAssembly(source);
                var testAssemblyAttribute = assembly.GetCustomAttributes(typeof(TestAssemblyAttribute), false).SingleOrDefault() as TestAssemblyAttribute;
                if (testAssemblyAttribute is null)
                {
                    continue;
                }

                // Now that the test assembly is found, look for methods which has the TestMethodAttribute
                var testAssembly = testAssemblyAttribute.Assembly;
                testCases.AddRange(FindTestsInAssembly(testAssembly));
            }
            return testCases;
        }

        private IEnumerable<TestCase> FindTestsInAssembly(Assembly assembly)
        {
            foreach (var exportedType in assembly.GetExportedTypes())
            {
                var publicMethods = exportedType.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var publicMethod in publicMethods)
                {
                    if (publicMethod.IsDefined(typeof(TestMethodAttribute), true))
                    {
                        var integrationTestAttribute = publicMethod.GetCustomAttribute<TestMethodAttribute>();
                        yield return new TestCase()
                        {
                            DisplayName = integrationTestAttribute.DisplayName ?? publicMethod.Name,
                            ExecutorUri = new Uri(TestExecutor.ExecutorUri),
                            FullyQualifiedName = $"{exportedType.FullName}.{publicMethod.Name}",
                            Source = assembly.Location
                        };
                    }
                }
            }
        }
    }
}
