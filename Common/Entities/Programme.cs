using System;

namespace Entities
{
    public class Programme
    {
        public string Title { get; set; }
        public string Episode { get; set; }
        public string Year { get; set; }
        public bool IsFilm { get; set; }
        public string Genre { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
    }
}
