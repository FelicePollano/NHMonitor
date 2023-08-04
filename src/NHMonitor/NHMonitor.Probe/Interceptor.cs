using Grpc.Core;
using Grpc.Net.Client;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using NHibernate;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.SqlCommand;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NHMonitor.Probe
{
    public class Interceptor:IAppender,IDisposable
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
            var hierarchy = (log4net.Repository.Hierarchy.Hierarchy)log4net.LogManager.GetRepository();
            hierarchy.Root.AddAppender(this);
            var sql = LogManager.GetLogger("NHibernate.SQL");
            BasicConfigurator.Configure(this);
            ((log4net.Repository.Hierarchy.Logger)sql.Logger).Level = Level.All;

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
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
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
            var msg = new InterceptData();
            msg.Type = MessageType.Bookmark;
            msg.Payload = bookmark;
            msg.Timestamp = GetTimeStamp();
            var _ = stream.WriteAsync(msg);
        }
        
        public void Close()
        {
        }
        public void DoAppend(LoggingEvent loggingEvent)
        {
            lock (this)
            {
                if (haveChannel && loggingEvent.LoggerName == "NHibernate.SQL")
                {
                    var msg = new InterceptData();
                    msg.Type = MessageType.Sql;
                    msg.Payload = loggingEvent.RenderedMessage;
                    msg.Timestamp = GetTimeStamp();
                    var _ = stream.WriteAsync(msg);
                }
            }
        }

        public void Dispose()
        {
            if (haveChannel)
            {
                
                channel.ShutdownAsync();
            }
        }
    }
}
