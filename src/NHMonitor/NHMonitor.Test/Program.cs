using System.Runtime.InteropServices;

var probe = new NHMonitor.Probe.Interceptor("test");
var receiver = new NHMonitor.Receiver.Listener();
receiver.StartServer();
Thread.Sleep(1000);
probe.OnPrepareStatement(null);
Console.ReadLine();
receiver.StopServer();
