using System;
using System.Collections.Generic;
using System.Text;

namespace NHMonitor.Receiver
{
    public interface IConsumer
    {
        void ApplicationRegistered(string appName);
        void ApplicationUnregistered(string appName);
        void Query(DateTime dt, string sql);
        void Bookmark(DateTime dt, string text);
    }
}
