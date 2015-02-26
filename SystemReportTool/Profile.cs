using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Diagnostics;
using System.IO;
using System.Threading;


namespace SystemReportTool
{
    class Profile
    {
        public Profile() 
        { 
        }

    }

    class ComputerSystem : Profile
    {
        private DataObject SystemResult = new DataObject();

        public ComputerSystem()
        {
            this.SystemResult.Name = "ComputerSystem";
        }

        public void Info()
        {
            WMI Profile = new WMI();
            Profile.SetParam("Win32_ComputerSystem");
            Profile.RunMetric();
            this.SystemResult.Items.Add(Profile.Package("Info"));
        }
        
        public void Enclosure()
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_systemenclosure");
            Profile.RunMetric();
            this.SystemResult.Items.Add(Profile.Package("Enclosure"));
        }

        public void Date()
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_bios");
            Profile.RunMetric();
            this.SystemResult.Items.Add(Profile.Package("Date"));
        }

        public DataObject GetResult()
        {
            return SystemResult;
        }

    }

    class Hardware : Profile 
    {
        private DataObject HardwareResult = new DataObject(); 

        public Hardware() 
        {
            this.HardwareResult.Name = "Hardware";          
        }

        public void Processor() 
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_processor");
            Profile.RunMetric();
            this.HardwareResult.Items.Add(Profile.Package("Processor"));
        }

        public void Memory()
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_physicalmemory");
            Profile.RunMetric();
            this.HardwareResult.Items.Add(Profile.Package("Memory"));
        }

        public void VideoController()
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_videocontroller");
            Profile.RunMetric();
            this.HardwareResult.Items.Add(Profile.Package("VideoController"));
        }

        public DataObject GetResult() 
        {
            return HardwareResult;
        }

    }

    class OperatingSystem : Profile
    {
        private DataObject OperatingSystemResult = new DataObject();

        public OperatingSystem() 
        {
            this.OperatingSystemResult.Name = "OperatingSystem";
        }

        public void OS() 
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_operatingsystem");
            Profile.RunMetric();
            this.OperatingSystemResult.Items.Add(Profile.Package("OS"));
        }

        public void Antivirus()
        {
            WMI Profile = new WMI("root\\SecurityCenter2");
            Profile.SetParam("AntivirusProduct");
            Profile.RunMetric();
            this.OperatingSystemResult.Items.Add(Profile.Package("Antivirus"));
        }

        public void Firewall()
        {
            WMI Profile = new WMI("root\\SecurityCenter2");
            Profile.SetParam("FirewallProduct");
            Profile.RunMetric();
            this.OperatingSystemResult.Items.Add(Profile.Package("Firewall"));
        }

        public void DxVersion()
        {
            Dictionary<string, string> DirectxVers = new Dictionary<string, string>();
            
            Native Profile = new Native();
            Process.Start("dxdiag", "/x dxv.xml");
            //Process.Start("dxdiag", "/x " + Environment.GetEnvironmentVariable("temp") + "\\dxv.xml");
            while (!File.Exists("dxv.xml"))
                Thread.Sleep(1000);
            XmlDocument doc = new XmlDocument();
            doc.Load("dxv.xml");
            XmlNode dxd = doc.SelectSingleNode("//DxDiag");
            XmlNode dxv = dxd.SelectSingleNode("//DirectXVersion");

            DirectxVers.Add("Version", Convert.ToString(dxv.InnerText.Split(' ')[1]));

            Profile.SetMetric(DirectxVers);
            this.OperatingSystemResult.Items.Add(Profile.Package("DirectX"));
            if (File.Exists(@"dxv.xml"))
            {
                File.Delete(@"dxv.xml");
            }

        }

        public DataObject GetResult()
        {
            return OperatingSystemResult;
        }
    }

    class Network : Profile 
    {
        private DataObject NetworkResult = new DataObject();

        public Network() 
        {
            this.NetworkResult.Name = "Network";
        }
        
        public void PingTest(string host, int count, string label)
        {
            Native Profile = new Native();
            Pinging SendPings = new Pinging();
            SendPings.SetParam(host, count, label);
            Profile.SetMetric(SendPings.Execute());
            this.NetworkResult.Items.Add(Profile.Package("Ping"));
        }

        public void TracertTest(string host, int hops)
        {
            Native Profile = new Native();
            Profile.SetMetric(Trace.Traceroute(host, hops));
            this.NetworkResult.Items.Add(Profile.Package("Tracert"));
        }

        public void PortTest(string host, int port, string label) 
        {
            Native Profile = new Native();
            TestPort SendTestPort = new TestPort();
            SendTestPort.SetParam(host, port, label);
            Profile.SetMetric(SendTestPort.Execute());
            this.NetworkResult.Items.Add(Profile.Package("Port"));
        }

        public DataObject GetResult()
        {
            return NetworkResult;
        }
    }
}
