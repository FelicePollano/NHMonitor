using Caliburn.Micro;
using NHMonitor.MockData;
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
using System.Windows;

namespace NHMonitor.ViewModels
{
    internal class MainWindowViewModel:Screen,IConsumer
    {
        readonly ObservableCollection<EventViewModel> events = new ObservableCollection<EventViewModel>();
        readonly ObservableCollection<string> applications = new ObservableCollection<string>();
        readonly Listener listener;

        public ObservableCollection<EventViewModel> Events => events;
        public MainWindowViewModel()
        {
            listener = new Listener(this);
            if (Debugger.IsAttached)
            {
                foreach(var k in MockdataGen.GenerateRandomSqls(100))
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
            Application.Current.Dispatcher.Invoke(
                () => events.Add(new EventViewModel(EventViewModel.Kind.sql) { Time = dt, Payload = sql })
                );
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            listener.StartServer();
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
