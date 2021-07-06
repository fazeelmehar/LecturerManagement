using System;
using System.Collections.Generic;
using System.Text;

namespace UniversityEnrollmentManager.Data.Settings
{
    public class ConnectionStrings
    {
        public string MSSQLDbs { get; set; }
    }

    public enum DataProvider
    {
        MSSQL
    }

    public class Data
    {
        public DataProvider Provider { get; set; }
    }
}
