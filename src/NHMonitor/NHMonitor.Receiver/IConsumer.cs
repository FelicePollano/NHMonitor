using System;
using System.Collections.Generic;
using System.Text;

namespace NHMonitor.Receiver
{
    public interface IConsumer
    {
        void ApplicationRegistered(string appName);
    }
}
