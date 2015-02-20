using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace SystemReportTool
{
    class NetworkTools
    {
    }

    class Pinging 
    {
        private string Data;
        private byte[] Buffer;
        private int Timeout;
        private int Count = 1;
        private string Host;
        

        public Pinging() 
        { 
            this.Data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            this.Timeout = 2000;
            this.Buffer = Encoding.ASCII.GetBytes(this.Data);
        }

        public void SetParam(string args)
        {
            this.Host = args;
        }

        public void SetParam(string args, int count)
        {
            this.Host = args;
            this.Count = count;
        }

        public Dictionary<string, string> Execute() 
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            int i = 0;
            int failedCounter = 0;
            string name;
            string value;
            Dictionary<string, string> PingCollection = new Dictionary<string, string>();
            PingCollection.Add("Host", this.Host);

            while (i < this.Count) 
            {
                PingReply reply = pingSender.Send(this.Host, this.Timeout, this.Buffer, options);
                
                if (reply.Status == IPStatus.Success)
                {
                    name = "Ping" + i;
                    value = "Dest : " + reply.Address.ToString() + " " + reply.RoundtripTime.ToString() + " ms " + "TTL " +  reply.Options.Ttl + " Buffer : " + reply.Buffer.Length;
                    PingCollection.Add(name, value);
                    
                }
                else 
                {
                    
                    name = "Ping" + i;
                    value = reply.Status.ToString();
                    PingCollection.Add(name, value);
                    failedCounter++;
                }

                i++;
            }
            PingCollection.Add("FailedInPourcent", Convert.ToString(((Convert.ToDouble(failedCounter) / Convert.ToDouble(this.Count)) * 100)));
            return PingCollection;
        }
    }
    public class TraceLocation {
		/// <summary>
		/// Hop number in a particular trace.
		/// </summary>
		public int Hop { get; set; }
		/// <summary>
		/// Time in milliseconds.
		/// </summary>
		public long Time { get; set; }
		/// <summary>
		/// IP address returned.
		/// </summary>
		public String IpAddress { get; set; }
	}

	public class Trace {
		/// <summary>
		/// Given an ip address or domain name, follow the trace path.
		/// 
		/// Idea and majority of the code from Jim Scott - http://coding.infoconex.com/post/C-Traceroute-using-net-framework.aspx
		/// </summary>
		/// <param name="ipAddressOrHostName">IP address or domain name to trace.</param>
		/// <param name="maximumHops">Maximum number of hops before quitting.</param>
		/// <returns>List of TraceLocation.</returns>

        private static Dictionary<string, string> TracertCollection = new Dictionary<string, string>();

        public static Dictionary<string, string> Traceroute(string ipAddressOrHostName, int maximumHops)
        {
			if (maximumHops < 1 || maximumHops > 100) {
				maximumHops = 30;
			}

			//IPAddress ipAddress = Dns.GetHostEntry(ipAddressOrHostName).AddressList[0];

			List<TraceLocation> traceLocations = new List<TraceLocation>();

			using (Ping pingSender = new Ping()) {
				PingOptions pingOptions = new PingOptions();
				Stopwatch stopWatch = new Stopwatch();
				byte[] bytes = new byte[32];
				pingOptions.DontFragment = true;
				pingOptions.Ttl = 1;

				for (int i = 1; i < maximumHops + 1; i++) {
					TraceLocation traceLocation = new TraceLocation();

					stopWatch.Reset();
					stopWatch.Start();
					PingReply pingReply = pingSender.Send(
						//ipAddress,
						ipAddressOrHostName,
                        5000,
						new byte[32], pingOptions);
					stopWatch.Stop();

					traceLocation.Hop = i;
					traceLocation.Time = stopWatch.ElapsedMilliseconds;
					if (pingReply.Address != null) {
						traceLocation.IpAddress = pingReply.Address.ToString();
					}

					traceLocations.Add(traceLocation);
					traceLocation = null;

					if (pingReply.Status == IPStatus.Success) {
						break;
					}
					pingOptions.Ttl++;
				}
			}
            TracertCollection.Clear();
            TracertCollection.Add("Host", ipAddressOrHostName);
            foreach(var trac in traceLocations)
            {
               
                TracertCollection.Add("Hop-" + Convert.ToString(trac.Hop), Convert.ToString(trac.Time) + " ms    " + trac.IpAddress);
            }
			return TracertCollection;
		}
	}
}
