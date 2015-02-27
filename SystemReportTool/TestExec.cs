using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SystemReportTool
{
    abstract class TestExec
    {
        public TestExec() 
        { 
        }

        abstract public void RunTest();
        abstract public List<DataObject> GetResult();
    }

    class TestFull:TestExec 
    {
        List<DataObject> ResultTestFull = new List<DataObject>();

        public TestFull() 
        {
            Request.ProgressBar.ProgressBarMax = 18;
            Request.ProgressBar.ProgressBarValue = 0;
        }
        
        public override void RunTest()
        {
            ComputerSystem TestSystem = new ComputerSystem();
            TestSystem.Info();
            Request.ProgressBar.ProgressBarValue++;
            TestSystem.Enclosure();
            Request.ProgressBar.ProgressBarValue++;
            TestSystem.Date();
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestFull.Add(TestSystem.GetResult());

            OperatingSystem TestOperatingSystem = new OperatingSystem();
            TestOperatingSystem.OS();
            Request.ProgressBar.ProgressBarValue++;
            TestOperatingSystem.Antivirus();
            Request.ProgressBar.ProgressBarValue++;
            TestOperatingSystem.Firewall();
            Request.ProgressBar.ProgressBarValue++;
            TestOperatingSystem.DxVersion();
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestFull.Add(TestOperatingSystem.GetResult());

            Hardware TestHardware = new Hardware();
            TestHardware.Processor();
            Request.ProgressBar.ProgressBarValue++;
            TestHardware.Memory();
            Request.ProgressBar.ProgressBarValue++;
            TestHardware.VideoController();
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestFull.Add(TestHardware.GetResult());

            Network TestNetwork = new Network();
            TestNetwork.PingTest("www.google.com", 10, "Google Website");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PingTest("8.8.8.8", 10, "Test IP Google");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PingTest("www.3dxchat.com", 10, "3DXChat Website");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PingTest("198.105.215.52", 10, "US Server");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.TracertTest("www.google.ca", 30);
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.TracertTest("www.3dxchat.com", 30);
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PortTest("198.105.215.52", 8124, "US Direct");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PortTest("54.72.64.172", 8124, "EU Proxy");
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestFull.Add(TestNetwork.GetResult());
        }

        public override List<DataObject> GetResult() 
        {
            return this.ResultTestFull;
        }
    }

    class TestHardware:TestExec
    {
        List<DataObject> ResultTestHardware = new List<DataObject>();
        
        public TestHardware() 
        {
            Request.ProgressBar.ProgressBarMax = 3;
            Request.ProgressBar.ProgressBarValue = 0;
        }
        
        public override void RunTest()
        {            
            Hardware TestHardware = new Hardware();
            TestHardware.Processor();
            Request.ProgressBar.ProgressBarValue++;
            TestHardware.Memory();
            Request.ProgressBar.ProgressBarValue++;
            TestHardware.VideoController();
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestHardware.Add(TestHardware.GetResult());
        }

        public override List<DataObject> GetResult()
        {
            return this.ResultTestHardware;
        }
    }

    class TestOperatingSystem : TestExec
    {
        List<DataObject> ResultTestOperatingSystem = new List<DataObject>();

        public TestOperatingSystem() 
        {
            Request.ProgressBar.ProgressBarMax = 4;
            Request.ProgressBar.ProgressBarValue = 0;
        }
        
        public override void RunTest()
        {
            List<DataObject> ResultTestOperatingSystem = new List<DataObject>();
         
            OperatingSystem TestOperatingSystem = new OperatingSystem();
            TestOperatingSystem.OS();
            Request.ProgressBar.ProgressBarValue++;
            TestOperatingSystem.Antivirus();
            Request.ProgressBar.ProgressBarValue++;
            TestOperatingSystem.Firewall();
            Request.ProgressBar.ProgressBarValue++;
            TestOperatingSystem.DxVersion();
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestOperatingSystem.Add(TestOperatingSystem.GetResult());
        }

        public override List<DataObject> GetResult()
        {
            return this.ResultTestOperatingSystem;
        }
    }

    class TestNetwork : TestExec
    {
        List<DataObject> ResultTestNetwork = new List<DataObject>();

        public TestNetwork() 
        {
            Request.ProgressBar.ProgressBarMax = 8;
            Request.ProgressBar.ProgressBarValue = 0;
        }

        public override void RunTest()
        {
            List<DataObject> ResultTestNetwork = new List<DataObject>();

            Network TestNetwork = new Network();
            TestNetwork.PingTest("www.google.com", 10, "Google Website");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PingTest("8.8.8.8", 10, "Test IP Google");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PingTest("www.3dxchat.com", 10, "3DXChat Website");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PingTest("198.105.215.52", 10, "US Server");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.TracertTest("www.google.ca", 30);
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.TracertTest("www.3dxchat.com", 30);
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PortTest("198.105.215.52", 8124, "US Direct");
            Request.ProgressBar.ProgressBarValue++;
            TestNetwork.PortTest("54.72.64.172", 8124, "EU Proxy");
            Request.ProgressBar.ProgressBarValue++;
            this.ResultTestNetwork.Add(TestNetwork.GetResult());
        }

        public override List<DataObject> GetResult()
        {
            return this.ResultTestNetwork; 
        }
    }
}
