using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemReportTool
{
    class Filter
    {
        private Dictionary<string, List<string>> Filters = new Dictionary<string, List<string>>();
        

        public Filter() 
        {
            this.Filters.Add("Info", new List<string>() { "Manufacturer", "Model", "SystemType", "ThermalState", "Status" });
            this.Filters.Add("Enclosure", new List<string>() { "Name", "Manufacturer", "ChassisTypes"});
            this.Filters.Add("OS", new List<string>() { "Caption", "OSArchitecture", "Version" });
            this.Filters.Add("Processor", new List<string>() { "DeviceID", "Name", "MaxClockSpeed", "SocketDesignation" });
            this.Filters.Add("Memory", new List<string>() { "Name", "Capacity", "BankLabel" });
            this.Filters.Add("VideoController", new List<string>() { "Name", "DriverVersion", "DriverDate" });
            this.Filters.Add("Date", new List<string>() { "ReleaseDate" });
           // this.Filters.Add("Ping", new List<string>() { "Host", "Ping0", "Ping1", "Ping2", "Ping3", "Ping4", "FailedInPourcent" });
            this.Filters.Add("Null", new List<string>() { "Null" });
        }

        public List<string> GetFilters(string filter)
        {
            if (!(this.Filters.ContainsKey(filter)))
            {
                return this.Filters["Null"];
            }
            else
            {
                return this.Filters[filter];
            }
            
        }
    }
}
