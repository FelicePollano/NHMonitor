using NUnit.Core;
using System.Reflection;
using System.Runtime.InteropServices;
/*
SimpleTestRunner runner = new SimpleTestRunner();
TestPackage package = new TestPackage("Test");
string loc = Assembly.GetExecutingAssembly().Location;
package.Assemblies.Add( loc );
if (runner.Load(package))
{
    TestResult result = runner.Run(new NullListener(),TestFilter.Empty,true,LoggingThreshold.All);
}
*/
Console.WriteLine("Launch unit tests from VStudio or NUnit.");