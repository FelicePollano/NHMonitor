
using Moq;
using NHMonitor.Probe;
using NHMonitor.Receiver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.Test
{
    [TestFixture]
    public class AppRegistrationListenFixture
    {
        const int syncDelay = 100;
        [Test]
        public async Task consumer_is_notified_when_app_is_registered()
        {
            var consumerMock = new Mock<IConsumer>();
            Listener receiver = new Listener(consumerMock.Object);
            receiver.StartServer();
            var probe = new Interceptor("test");
            await Task.Delay(syncDelay); //let channel synchronize
            consumerMock.Verify(u => u.ApplicationRegistered("test"),Times.Once);
        }
    }
}
