using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SystemReportTool
{
    public class DataObject
    {  
        //private Dictionary<string, string> Attributes; 
        //private List<DataObject> ChildItem = new List<DataObject>();

        public DataObject()
        {
            this.Attributes = new Dictionary<string, string>();
            this.Items = new List<DataObject>();
        }

        public void InitItems() 
        {
            this.Items.Add(new DataObject());
        }

        public string Name { get; set; } 
        public Dictionary<string, string> Attributes { get; set; }
        public List<DataObject> Items { get; set; }

    }
}
