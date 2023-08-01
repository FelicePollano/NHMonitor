using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.ViewModels
{
    public class EventViewModel
    {
        public EventViewModel(Kind kind)
        {
            Type = kind;
        }
        public enum Kind
        {
            sql
        }
        // we should not do back in time :) but bugs exists so

        public string DeltaString => string.Format("{0:+0000} ms", Delta);
        public int Delta { get; set; }
        public DateTime Time { get; set; }
        public string Payload { get; set; }
        public Kind Type { get; private set; }
        public string Title => $"{Type}-{Time}";
        public AbstractEventViewModel EventModel { 
            get
            {
                switch (Type)
                {
                    case Kind.sql:
                        return new QueryEventViewModel(Type.ToString()) { QueryText=Payload};
                }
                return null;
            }
        }
    }
}
