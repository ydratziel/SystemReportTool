using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SystemReportTool
{
    class BuildReport
    {

        private List<DataObject> Data;
        private XDocument Report;
        private XElement Items;

        public BuildReport() 
        {
            this.Report = new XDocument(new XDeclaration("1.0", "utf-8", null));
            this.Items = new XElement("Report");
        }

        public void Load(List<DataObject> data)
        {
            this.Data = data;
        }

        public void ReportInit() 
        {
            XElement header = new XElement("Header");
            XElement subheader = new XElement("Reporting");
            subheader.Add(new XElement("Version", "Beta 0.5.0"));
            subheader.Add(new XElement("Date", DateTime.Now.ToString()));
            header.Add(subheader);
            this.Items.Add(header);
        }

        public void ReportMain() 
        {
            Filter ReportFilter = new Filter();

            foreach (var part in this.Data) 
            {
                XElement element2 = new XElement(part.Name);
                foreach (var subpart in part.Items) 
                {
                    if (subpart.Attributes.Count == 0) 
                    {
                        foreach (var subsubpart in subpart.Items) 
                        {
                            XElement element3 = new XElement(subsubpart.Name);
                            var NotExist = ReportFilter.GetFilters(subsubpart.Name);
                            if (NotExist[0] == "Null")
                            {
                                foreach (KeyValuePair<string,string> value in subsubpart.Attributes) 
                                {
                                    element3.Add(new XElement(value.Key, value.Value));
                                }
                            }
                            else
                            {
                                foreach (var filter in ReportFilter.GetFilters(subsubpart.Name))
                                {
                                    element3.Add(new XElement(filter, subsubpart.Attributes[filter]));
                                }
                            }
                            element2.Add(element3);
                        }
                    }
                }
                this.Items.Add(element2);
            }
            this.Report.Add(this.Items);
        }

        public XDocument GetReport() 
        {
            return this.Report;
        }
    }
}
