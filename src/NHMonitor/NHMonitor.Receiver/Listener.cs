using Grpc.Core;
using NHMonitor.Probe;
using System;
using System.Threading.Tasks;

namespace NHMonitor.Receiver
{
    public class Listener:NHMonitorService.NHMonitorServiceBase
    {
        int Port = 6925;
        Server server;
        public Listener()
        {
            
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
        public void StopServer()
        {
            server.ShutdownAsync().Wait();
        }
        public override Task<Ack> Register(RegisterApp request, ServerCallContext context)
        {
            return Task.FromResult(new Ack());
        }
        public override async Task<Ack> MonitorStream(IAsyncStreamReader<InterceptData> requestStream, ServerCallContext context)
        {
            do
            {
                Console.WriteLine("received msg");
            } while (await requestStream.MoveNext());
            return new Ack();
        }
    }
}
