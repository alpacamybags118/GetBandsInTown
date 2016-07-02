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
            eventTimer.Interval = 60;
     
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
            var reader = new StreamReader("bandstosearch");
            var requestString = "http://api.bandsintown.com/events/search?";
            var result = string.Empty;

            //add bands from list (50 max)
            while (!reader.EndOfStream)
            {
                var band = String.Empty;
                band = reader.ReadLine();

                band.Replace(" ", "+");
                requestString += "artist[]=" + band + "&";
            }

            reader.Close();
            requestString += "location=Lansing,MI&radius=100&format=json&app_id=GetShowsInTown";

            request = WebRequest.Create(requestString);
            request.UseDefaultCredentials = true;
            request.Method = "GET";

            try
            {
                reader = new StreamReader(request.GetResponse().GetResponseStream());
                result = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                throw e;
            }

            return new List<Event>();
        }
    }
}
