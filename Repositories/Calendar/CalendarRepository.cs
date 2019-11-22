using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiFramework.Data.LeasePak;

namespace ReportingDemo.Repositories.Calendar
{
    public class CalendarRepository : ICalendarRepository
    {
        public CalendarRepository()
        {
            LeasePak.Init(RuntimeSettings.ConnectionStrings.LeasePakConnectionString);
        }

        public DateTime GetPreviousBusinessDay(DateTime businessDay)
        {
            return LeasePak.Calendar.GetPreviousBusinessDay(businessDay);
        }
    }
}