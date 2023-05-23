using Grpc.Core;
using Grpc.Net.Client;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NHMonitor.Probe
{
    public class Interceptor:EmptyInterceptor,IAppender
    {
        private readonly string appName;
        private readonly Channel channel;
        int appId;
        NHMonitorService.NHMonitorServiceClient client;
        IClientStreamWriter<InterceptData> stream;
        bool haveChannel;

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Interceptor(string appName)
        {
            BasicConfigurator.Configure(this);
            this.appName = appName;
            channel = new Channel("localhost:6925", ChannelCredentials.Insecure);
            client = new NHMonitorService.NHMonitorServiceClient(channel);
            Task.Run( async () => {
                while (!haveChannel)
                {
                    await EstablishChannel();
                }
            });
        }
        private long GetTimeStamp()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        private async Task EstablishChannel()
        {
            try
            {
                RegisterAck ack = await client.RegisterAsync(new RegisterApp() { AppName = appName });
                appId = ack.AppId;
                if (appId != 0)
                {
                    haveChannel = true;
                    stream = client.MonitorStream(new CallOptions() { }).RequestStream;
                }
            }
            catch(Exception e)
            {
                await Task.Delay(1000);
            }
            finally
            {

            }
                                
        }
        public void SendBookmark(string bookmark)
        {

        }
        public void Close()
        {
        }
        public void DoAppend(LoggingEvent loggingEvent)
        {
            if (loggingEvent.LoggerName == "NHibernate.SQL")
            {
                var msg = new InterceptData();
                msg.Type = MessageType.Sql;
                msg.Payload = loggingEvent.RenderedMessage;
                msg.Timestamp = GetTimeStamp();
                var _ = stream.WriteAsync(msg);
            }
        }
    }
}
