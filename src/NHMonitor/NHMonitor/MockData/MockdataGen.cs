using NHMonitor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.MockData
{
    public static class MockdataGen
    {
        static public IEnumerable<EventModel> GenerateRandomSqls(int count)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new EventModel(EventModel.Kind.sql) { Time = DateTime.Now, Payload = "SELECT * FROM ANYTHING" };
            }
        }
    }
}
