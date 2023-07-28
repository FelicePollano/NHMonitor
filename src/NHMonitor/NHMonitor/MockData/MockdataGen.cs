using NHMonitor.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHMonitor.MockData
{
    public static class MockdataGen
    {
        static public IEnumerable<EventViewModel> GenerateRandomSqls(int count)
        {
            for (int i = 0; i < count; i++)
            {
                //https://stackoverflow.com/questions/4255398/wpf-avalonedit-sql-xhsd-request
                yield return new EventViewModel(EventViewModel.Kind.sql) { Time = DateTime.Now, Payload = @"SELECT * FROM ANYTHING
INNER JOIN OTHER ON OTHER.X=ANYTHING.Y
WHERE D>1000
AND Q>0
GROUP BY SN
ORDER BY BLA
" };
            }
        }
    }
}
