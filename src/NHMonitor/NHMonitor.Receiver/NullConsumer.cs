using System;
using System.Collections.Generic;
using System.Text;

namespace NHMonitor.Receiver
{
    public class NullConsumer : IConsumer
    {
        internal NullConsumer()
        {
            
        }
        public void ApplicationRegistered(string appName)
        {
            
        }

        public void ApplicationUnregistered(string appName)
        {
            
        }

        public void Bookmark(DateTime dt, string text)
        {
            
        }

        public void Query(DateTime dt, string sql)
        {
            
        }
    }
}
