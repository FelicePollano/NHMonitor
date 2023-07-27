using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.ViewModels
{
    public abstract class AbstractEventViewModel
    {
        public AbstractEventViewModel(string header)
        {
            Title = header;
        }
        public string Title { get; set; }
    }
}
