using System.Collections.Generic;
namespace Entities
{
    public class Channel
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public List<Programme> Programmes { get; set; }
    }
}
