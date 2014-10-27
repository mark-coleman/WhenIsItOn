﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Listing
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Episode { get; set; }
        public string Year { get; set; }
        public bool IsFilm { get; set; }
        public string Genre { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public Channel Channel { get; set; }
    }
}
