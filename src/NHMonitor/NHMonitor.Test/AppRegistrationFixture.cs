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

#pragma warning disable 8605, 8602,8618
namespace NHMonitor.Test
{
    [TestFixture]
    public class AppRegistrationFixture
    {
        Interceptor probe;
        Listener receiver;
        const int syncDelay = 100;
        [SetUp]
        public void Setup()
        {
            
            receiver = new Listener(Listener.NullConsumer);
            receiver.StartServer();
        }

        [Test]
        public async Task registration_should_obtain_an_appID()
        {
            probe = new Interceptor("1test");
            await Task.Delay(syncDelay); //let channel synchronize
            /*
             * Not testing the obvious strategy here.
             * This we need to test, but definitely that value must not be part of
             * the public interface....
             */
            int appId = (int)probe.GetType()
                .GetField("appId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(probe);
            Assert.NotZero(appId);
        }
        [Test]
        public async Task multiple_registration_should_obtain_different_appID()
        {
            int spawnCount = 10;
            HashSet<int> appIDs = new HashSet<int>();
            for (int i = 0; i < spawnCount; ++i)
            {
                probe = new Interceptor($"2test{i}");
                await Task.Delay(syncDelay); //let channel synchronize
                /*
                 * Not testing the obvious strategy here.
                 * This we need to test, but definitely that value must not be part of
                 * the public interface....
                 */
                int appId = (int)probe.GetType()
                    .GetField("appId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(probe);
                Assert.NotZero(appId);
                Assert.False(appIDs.Contains(appId));
                appIDs.Add(appId);
            }
        }
        [Test]
        
        public async Task multiple_registration_with_same_id_should_fail()
        {
            var probe = new Interceptor($"3test");
            await Task.Delay(syncDelay); //let channel synchronize
            probe = new Interceptor($"3test");
            await Task.Delay(syncDelay);
            int appId = (int)probe.GetType()
                    .GetField("appId", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(probe);
            Assert.Zero(appId);
        }

        [TearDown]
        public async Task ShutDown()
        {
            await receiver.StopServer();
        }
    }
}
