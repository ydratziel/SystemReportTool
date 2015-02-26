using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace SystemReportTool
{

    abstract class Metric
    {   
        public Metric() 
        { 
        
        }

        public abstract void SetParam(string First);
        public abstract void SetParam(string First, string Second);
        public abstract void RunMetric();
        public abstract DataObject Package(string packageName);
    }

    class WMI:Metric 
    {
        private string provider;
        private string condition;
        private ManagementScope scope;
        private ManagementObjectCollection metric;

        public WMI() 
        {
            this.scope = new ManagementScope("root\\CIMV2");
        }

        public WMI(string scope) 
        {
            this.scope = new ManagementScope(scope);
        }

        public override void SetParam(string provider)
        {
            this.provider = provider;
        }

        public override void SetParam(string provider, string condition)
        {
            this.provider = provider;
            this.condition = condition;
        }

        public override void RunMetric() 
        {
            string query; 
            //ManagementObject obj = new ManagementObject();

            if (String.IsNullOrEmpty(this.condition))
            {
                query = "SELECT * FROM " + this.provider;
            }
            else
            {
                query = "SELECT * FROM " +  this.provider + " WHERE " + this.condition;
            }

            SelectQuery q = new SelectQuery(query);

            ManagementObjectSearcher mosQuery = new ManagementObjectSearcher(this.scope, q);
            ManagementObjectCollection queryCollection = mosQuery.Get();
                                
            this.metric = queryCollection;
        }

        public override DataObject Package(string packageName) 
        {
            DataObject package = new DataObject();
            int i = 0;
            foreach (ManagementBaseObject item in this.metric) 
            {
                package.InitItems();
                
                var itemsProperties = item.Properties;
                
                foreach(var properties in itemsProperties)
                {    
                    string name = properties.Name.ToString();
                    var value = properties.Value;

                    package.Items[i].Attributes.Add(name, Convert.ToString(value));
                    package.Items[i].Name = packageName;
                }
                i++;
            }
            return package;
        }
    }

    class Native
    {
        private string Parameter;
        private string Extra;
        private Dictionary<string, string> MetricValue;

        public Native()
        {

        }
        
        public void SetParam(string param)
        {
            this.Parameter = param;
        }

        public void SetParam(string param, string option)
        {
            this.Parameter = param;
            this.Extra = option;
        }

        public void SetMetric(Dictionary<string,string> metricVal)
        {
            this.MetricValue = metricVal;
        }

        public DataObject Package(string packageName)
        {
            DataObject package = new DataObject();
            int i = 0;
            package.InitItems();

            foreach (KeyValuePair<string, string> item in this.MetricValue)
                {
          
                    string name = item.Key;
                    var value = item.Value;

                    package.Items[0].Attributes.Add(name, Convert.ToString(value));
                    package.Items[0].Name = packageName;
                    i++;
                }
            return package;
        }


    }

}
