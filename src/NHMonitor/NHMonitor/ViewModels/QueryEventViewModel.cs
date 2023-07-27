using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.ViewModels
{
    public class QueryEventViewModel:AbstractEventViewModel
    {
        public QueryEventViewModel(string headr)
            :base(headr)
        {
            
        }
        public string QueryText { get; set; }
    }
}
