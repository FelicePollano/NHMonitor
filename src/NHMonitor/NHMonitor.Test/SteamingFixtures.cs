
using Moq;
using NHibernate.SqlCommand;
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
    public class SteamingFixtures
    {
        Listener receiver;
        Mock<IConsumer> consumerMock;
        const int syncDelay = 100;
        const int commDelay = 5;
        [SetUp]
        public void Setup()
        {
            consumerMock = new Mock<IConsumer>();
            receiver = new Listener(consumerMock.Object);
            receiver.StartServer();
        }
        [Test]
        public async Task test_sql_message_is_reveived()
        {
            var probe = new Interceptor($"1test");
            await Task.Delay(syncDelay);
            var sql = new SqlString("select * from somewhere");
            sql.Append(new SqlString(Parameter.WithIndex(1)));
            probe.OnPrepareStatement(sql);
            await Task.Delay(commDelay);
            consumerMock.Verify(k => k.Query(It.IsAny<DateTime>(), "select * from somewhere", It.IsAny<IEnumerable<KeyValuePair<string, string>>>()),Times.Once);

        }

        [TearDown]
        public async Task ShutDown()
        {
            await receiver.StopServer();
        }
    }
}
