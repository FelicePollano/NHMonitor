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

        public void Query(DateTime dt, string sql, IEnumerable<KeyValuePair<string, string>> parameters)
        {
            
        }
    }
}
