﻿using System;
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

        public void Date()
        {
            WMI Profile = new WMI();
            Profile.SetParam("win32_bios");
            Profile.RunMetric();
            this.OperatingSystemResult.Items.Add(Profile.Package("Date"));
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
        
        public void PingTest(string host, int count)
        {
            Native Profile = new Native();
            Pinging SendPings = new Pinging();
            SendPings.SetParam(host, count);
            Profile.SetMetric(SendPings.Execute());
            this.NetworkResult.Items.Add(Profile.Package("Ping"));
        }

        public void TracertTest(string host, int hops)
        {
            Native Profile = new Native();
            Profile.SetMetric(Trace.Traceroute(host, hops));
            this.NetworkResult.Items.Add(Profile.Package("Tracert"));
        }

        public DataObject GetResult()
        {
            return NetworkResult;
        }
    }
}