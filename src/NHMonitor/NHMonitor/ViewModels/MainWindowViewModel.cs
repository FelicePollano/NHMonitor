using Caliburn.Micro;
using NHMonitor.Models;
using NHMonitor.Receiver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NHMonitor.ViewModels
{
    internal class MainWindowViewModel:Screen,IConsumer
    {
        ObservableCollection<EventModel> events = new ObservableCollection<EventModel>();
        ObservableCollection<string> applications = new ObservableCollection<string>();
        public void ApplicationRegistered(string appName)
        {
            applications.Add(appName);
        }

        public void Query(DateTime dt, string sql)
        {
            events.Add(new EventModel(EventModel.Kind.sql) { Time = dt, Payload = sql });
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            Listener listener = new Listener(this);
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
