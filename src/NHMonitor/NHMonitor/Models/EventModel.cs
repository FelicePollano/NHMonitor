using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.Models
{
    public class EventModel
    {
        public EventModel(Kind kind)
        {
            Type = kind;
        }
        public enum Kind
        {
            sql
        }
        public DateTime Time { get; set; }
        public string  Payload { get; set; }
        public Kind Type { get; private set; }
    }
}
