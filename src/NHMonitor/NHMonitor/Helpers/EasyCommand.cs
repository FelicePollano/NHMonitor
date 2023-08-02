using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NHMonitor.Helpers
{
    public class EasyCommand:ICommand
    {
        Func<object?, bool> canCommand = (k)=>true;
        Action<object?> command;
       
        public EasyCommand(Action<object?> command,Func<object?,bool> canCommand)
        {
            this.command = command;
            this.canCommand = canCommand;
        }
        public EasyCommand(Action<object> command)
        {
            this.command = command;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return canCommand(parameter);
        }

        public void Execute(object? parameter)
        {
            command(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this,EventArgs.Empty);
        }
    }
}
