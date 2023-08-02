using Caliburn.Micro;
using NHMonitor.Helpers;
using NHMonitor.MockData;
using NHMonitor.Receiver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
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
        readonly BasicFormatter formatter = new BasicFormatter();
        readonly Listener listener;
        int maxEvents = 500;
        public EasyCommand ClearCommand { get; private set; }
        public ObservableCollection<EventViewModel> Events => events;
        public MainWindowViewModel()
        {
            listener = new Listener(this);
            ClearCommand = new EasyCommand( (m) =>
                                        {
                                            events.Clear();
                                            ClearCommand.RaiseCanExecuteChanged();
                                        }
                                        ,(m) => events.Count > 0 );
            if (Debugger.IsAttached)
            {
                foreach(var k in MockdataGen.GenerateRandomSqls(100))
                {
                    events.Add(k);
                }
            }
            
        }
        private int appCount;

        public int AppCount
        {
            get { return appCount; }
            set { appCount = value; NotifyOfPropertyChange(nameof(AppCount)); }
        }
        
        public void ApplicationRegistered(string appName)
        {
            applications.Add(appName);
            AppCount++;
        }
        protected override async Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            await listener.StopServer();
            await base.OnDeactivateAsync(close, cancellationToken);
        }
        public void Query(DateTime dt, string sql)
        {
            Application.Current.Dispatcher.Invoke(
                () => {
                    var @event = new EventViewModel(EventViewModel.Kind.sql) { Time = dt, Payload = formatter.Format(sql) };
                    if (events.Count > 0)
                    {
                        @event.Delta =(int) (dt - events[0].Time).TotalMilliseconds;
                    }
                    events.Insert(0, @event);
                    if(events.Count > maxEvents)
                    {
                        events.RemoveAt(events.Count - 1);
                    }
                    ClearCommand.RaiseCanExecuteChanged();
                }
                );
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            listener.StartServer();
            return base.OnActivateAsync(cancellationToken);
        }
    }
}
