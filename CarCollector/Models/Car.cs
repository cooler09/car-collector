using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCollector.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Series { get; set; }
        public int ModelId { get; set; }
        public int Year { get; set; }
    }
}
