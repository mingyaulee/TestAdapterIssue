using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using System;
using System.Collections.Generic;

namespace VS.TestAdapter
{
    [ExtensionUri(ExecutorUri)]
    public class TestExecutor : ITestExecutor
    {
        public const string ExecutorUri = "executor://MyTestExecutor";

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            // Just simulating all passing test result
            foreach (var testCase in tests)
            {
                var testResult = new TestResult(testCase)
                {
                    Outcome = TestOutcome.Passed
                };
                frameworkHandle.RecordResult(testResult);
            }
        }

        public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
        {
            // Look for test cases in assemblies
            var testCases = new TestDiscoverer().FindTestCases(sources);
            // Just simulating all passing test result
            foreach (var testCase in testCases)
            {
                var testResult = new TestResult(testCase)
                {
                    Outcome = TestOutcome.Passed
                };
                frameworkHandle.RecordResult(testResult);
            }
        }
    }
}
