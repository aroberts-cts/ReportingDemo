using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingDemo.Repositories.Calendar
{
    public interface ICalendarRepository
    {
        DateTime GetPreviousBusinessDay(DateTime businessDay);
    }
}
