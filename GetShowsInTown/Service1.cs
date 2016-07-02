using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace GetShowsInTown
{
    public partial class Service1 : ServiceBase
    {
        Timer eventTimer;

        public Service1()
        {
            InitializeComponent();

            eventTimer = new Timer();
        }

        protected override void OnStart(string[] args)
        {
            //eventTimer.Interval = 3600000;
            eventTimer.Interval = 60000;


            eventTimer.Elapsed += new ElapsedEventHandler(eventTimerTick);
            eventTimer.Start();
        }

        protected override void OnStop()
        {
            eventTimer.Stop();
        }

        private void eventTimerTick(object arg, ElapsedEventArgs e)
        {
            var eventsInTown = new List<Event>();

            eventsInTown = GetEventsInTown();
        }

        private List<Event> GetEventsInTown()
        {
            WebRequest request;
            string[] bands = Resources.BandNames.Split(',');
            var requestString = String.Empty;
            var result = string.Empty;
            StreamReader reader;

            //add bands from list (50 max)
            foreach (string band in bands)
            {
                band.Replace(" ", "+");
                requestString = string.Format("http://api.bandsintown.com/artists/{0}/events/search.json?api_version=2.0&app_id=GetShowsInTown&location=Lansing,MI&radius=100", band);


                request = WebRequest.Create(requestString);
                request.UseDefaultCredentials = true;
                request.Method = "GET";

                try
                {
                    reader = new StreamReader(request.GetResponse().GetResponseStream());
                    result = reader.ReadToEnd();

                    var events = JsonConvert.DeserializeObject<Event>(result);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            return new List<Event>();
        }
    }
}
