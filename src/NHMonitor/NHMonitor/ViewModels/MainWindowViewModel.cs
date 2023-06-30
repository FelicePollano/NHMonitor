using Caliburn.Micro;
using NHMonitor.MockData;
using NHMonitor.Models;
using NHMonitor.Receiver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NHMonitor.ViewModels
{
    internal class MainWindowViewModel:Screen,IConsumer
    {
        readonly ObservableCollection<EventModel> events = new ObservableCollection<EventModel>();
        readonly ObservableCollection<string> applications = new ObservableCollection<string>();
        readonly Listener listener;

        public IEnumerable<EventModel> Events => events;
        public MainWindowViewModel()
        {
            listener = new Listener(this);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                foreach(var k in MockdataGen.GenerateRandomSqls(1000))
                {
                    events.Add(k);
                }
            }
        }
        public void ApplicationRegistered(string appName)
        {
            applications.Add(appName);
        }
        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            await listener.StopServer();
            await base.OnDeactivateAsync(close, cancellationToken);
        }
        public void Query(DateTime dt, string sql)
        {
            events.Add(new EventModel(EventModel.Kind.sql) { Time = dt, Payload = sql });
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            listener.StartServer();
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
