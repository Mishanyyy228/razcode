using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab.model
{
    public class TodoModel
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleated { get; set; }
        public Coordinate Coordinate { get; set; }
    }
}
    public class Coordinate
    {
        public string longitude { get; set; }
        public string latitude { get; set; }
    }
    


