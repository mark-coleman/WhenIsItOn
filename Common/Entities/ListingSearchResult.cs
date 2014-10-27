using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ListingSearchResult
    {
        public string Title { get; set; }
        public string StartTime { get; set; }
        public DateTime Date { get; set; }
        public string ChannelName { get; set; }
        public string DisplayStartDateTime { get; set; }
    }
}
