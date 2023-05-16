using Grpc.Core;
using NHMonitor.Probe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHMonitor.Receiver
{
    public class Listener:NHMonitorService.NHMonitorServiceBase
    {
        int Port = 6925;
        Server server;
        int nextAppId = 0;
        HashSet<string> apps;
        readonly IConsumer consumer;
        public static IConsumer NullConsumer = new NullConsumer();
        public Listener(IConsumer consumer)
        {
            apps = new HashSet<string>();
            this.consumer = consumer;
        }
        public void StartServer()
        {
            server = new Server
            {
                Services = { NHMonitorService.BindService(this) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();
        }
        public async Task StopServer()
        {
            await server.KillAsync();
        }
        public override Task<RegisterAck> Register(RegisterApp request, ServerCallContext context)
        {
            lock (this)
            {
                if (!apps.Contains(request.AppName))
                {
                    apps.Add(request.AppName);
                    consumer.ApplicationRegistered(request.AppName);
                    return Task.FromResult(new RegisterAck() { AppId = ++nextAppId });
                }
                else
                {
                    return Task.FromResult(new RegisterAck() { AppId = 0,Message=$"application {request.AppName} already registered" });
                }
            }
        }
        public override async Task<Ack> MonitorStream(IAsyncStreamReader<InterceptData> requestStream, ServerCallContext context)
        {
            do
            {
                var msg = requestStream.Current;
                switch (msg.Type)
                {
                    
                }
            } while (await requestStream.MoveNext());
            return new Ack();
        }
    }
}
