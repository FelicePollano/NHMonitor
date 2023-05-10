using NHMonitor.Probe;
using NHMonitor.Receiver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public void registration_should_obtain_an_appID()
        {
            probe = new Interceptor("test");
            Thread.Sleep(1000); //let channel synchronize
            Assert.NotZero(probe.AppId);
        }

        [TearDown]
        public async Task ShutDown()
        {
            await receiver.StopServer();
        }
    }
}
