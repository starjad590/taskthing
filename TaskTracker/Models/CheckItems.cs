using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Models
{
    public class CheckItems
    {
        public int id { get; set; }
        public string thingToCheck { get; set; }
        public bool isCompleted { get; set; }
    }
}
