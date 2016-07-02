using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetShowsInTown
{
    public class Event
    {
        public int id;
        public string title;
        public DateTime datetime;
        public string formatted_datetime;
        public string formatted_location;
        public string ticket_url;
        public string ticket_type;
        public string ticket_status;
        public DateTime on_sale_datetime;

    }
}
