using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace SystemReportTool
{
    public partial class Request : Form
    {

        private XDocument XmlReport;
        private List<DataObject> ListData;
        private List<Node> Data = new List<Node>();
        private BrightIdeasSoftware.TreeListView treeListView;
        private Node parents;

        public Request()
        {
            InitializeComponent();
            AddTree();
            backgroundWorker2.WorkerReportsProgress = true;
            radioButton1.Checked = true;
        }
        
        public static class ProgressBar
        {
            public static int ProgressBarMax { get; set; }
            public static int ProgressBarValue { get; set; }
        }

        // embedded class
        class Node
        {
            public string Name { get; private set; }
            public string Value { get; private set; }
            public List<Node> Children { get; private set; }
            public Node(string name, string col1)
            {
                this.Name = name;
                this.Value = col1;
                this.Children = new List<Node>();
            }
        }

        // private methods
        private void FillTree()
        {
            // set the delegate that the tree uses to know if a node is expandable
            this.treeListView.CanExpandGetter = x => (x as Node).Children.Count > 0;
            // set the delegate that the tree uses to know the children of a node
            this.treeListView.ChildrenGetter = x => (x as Node).Children;

            // create the tree columns and set the delegates to print the desired object proerty
            var nameCol = new BrightIdeasSoftware.OLVColumn("Name", "Name");
            nameCol.AspectGetter = x => (x as Node).Name;

            var col1 = new BrightIdeasSoftware.OLVColumn("Value", "Value");
            col1.AspectGetter = x => (x as Node).Value;

            nameCol.MinimumWidth = 200;
            col1.MinimumWidth = 300;

            // add the columns to the tree
            this.treeListView.Columns.Add(nameCol);
            this.treeListView.Columns.Add(col1);

            // set the tree roots
            this.treeListView.Roots = this.Data;
        }

        private void InitializeData()
        {
            int x = 0;
            int y = -1;
            XmlReader rdr = XmlReader.Create(new StringReader(this.XmlReport.ToString()));
            while (rdr.Read())
            {
                if (rdr.NodeType == XmlNodeType.Element)
                {
                    switch (rdr.Depth)
                    {
                        case 0:
                            break;
                        case 1:
                            if (x > 0)
                            {
                                this.Data.Add(this.parents);
                            }
                            y = -1;
                            this.parents = new Node(rdr.Name, "-");
                            x++;
                            break;
                        case 2:
                            this.parents.Children.Add(new Node(rdr.Name, "-"));
                            y++;
                            break;
                        default:
                            this.parents.Children[y].Children.Add(new Node(rdr.Name, rdr.ReadElementContentAsString()));
                            break;
                    }
                }
            }
            this.Data.Add(this.parents);
        }

        private void AddTree()
        {
            treeListView = new BrightIdeasSoftware.TreeListView();
            treeListView.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(treeListView);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked)
            {
                //Full
                TestFull Data = new TestFull();
                
                backgroundWorker1.RunWorkerAsync(Data);
                backgroundWorker2.RunWorkerAsync();
            }
            else if (radioButton5.Checked)
            {
                //OS
                TestOperatingSystem Data = new TestOperatingSystem();
                
                backgroundWorker1.RunWorkerAsync(Data);
                backgroundWorker2.RunWorkerAsync();
            }
            else if (radioButton3.Checked)
            {
                //Hardware
                TestHardware Data = new TestHardware();
                
                backgroundWorker1.RunWorkerAsync(Data);
                backgroundWorker2.RunWorkerAsync();
            }

            else if (radioButton2.Checked)
            {
                //Network
                TestNetwork Data = new TestNetwork();

                backgroundWorker1.RunWorkerAsync(Data);
                backgroundWorker2.RunWorkerAsync();
            }
            button2.Enabled = false;
            clearDataToolStripMenuItem.Enabled = true;
            
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var myAboutBox = new AboutBox1();
            myAboutBox.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string dtf = DateTime.Now.ToString("yyyyMMdd");
            this.XmlReport.Save("Report-" + dtf + ".xml");
        }

        private void xMLReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Show the dialog and get result.
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                var x = openFileDialog1.FileName;
                this.XmlReport = XDocument.Load(x);
                InitializeData();
                FillTree();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            TestExec argumentTest = e.Argument as TestExec;
            argumentTest.RunTest();

            e.Result = argumentTest;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TestExec resultTest = e.Result as TestExec;
            this.ListData = resultTest.GetResult();

            BuildReport Report = new BuildReport();
            Report.Load(this.ListData);
            Report.ReportInit();
            Report.ReportMain();
            this.XmlReport = Report.GetReport();

            InitializeData();
            FillTree();

            button3.Enabled = true;

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            while(backgroundWorker1.IsBusy)
            {
                backgroundWorker2.ReportProgress(Convert.ToInt32(((Convert.ToDouble(ProgressBar.ProgressBarValue) / Convert.ToDouble(ProgressBar.ProgressBarMax)) * 100)));
            }
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void clearDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            progressBar1.Value = 0;
            XmlReport = null;
            ListData.Clear();
            Data.Clear();
            treeListView.Clear();
            clearDataToolStripMenuItem.Enabled = false;
            button3.Enabled = false;
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void Request_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

    }
}
