using NHMonitor.Probe;
using NHMonitor.Receiver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.Test
{
    [TestFixture]
    public class AppRegistrationFixture
    {
        Interceptor probe;
        Listener receiver;
        

        [SetUp]
        public void Setup()
        {
            
            receiver = new Listener();
            receiver.StartServer();
        }

        [Test]
        public async Task registration_should_obtain_an_appID()
        {
            probe = new Interceptor("test");
            await Task.Delay(50); //let channel synchronize
            /*
             * Not testing the obvious strategy here.
             * This we need to test, but definitely that value must not be part of
             * the public interface....
             */
            int appId = (int)probe.GetType()
                .GetField("appId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(probe);
            Assert.NotZero(appId);
        }

        [TearDown]
        public async Task ShutDown()
        {
            await receiver.StopServer();
        }
    }
}
