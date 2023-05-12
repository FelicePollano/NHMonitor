using Grpc.Core;
using Grpc.Net.Client;
using NHibernate;
using NHibernate.SqlCommand;
using System;
using System.Threading.Tasks;

namespace NHMonitor.Probe
{
    public class Interceptor:EmptyInterceptor
    {
        private readonly string appName;
        private readonly Channel channel;
        int appId;
        NHMonitorService.NHMonitorServiceClient client;
        IClientStreamWriter<InterceptData> stream;
        bool haveChannel;
        public Interceptor(string appName)
        {
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
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            var s =  base.OnPrepareStatement(sql);
            if (haveChannel)
            {
                var _ = stream.WriteAsync(new InterceptData { Sql = null });
            }
            return s;
        }
    }
}
