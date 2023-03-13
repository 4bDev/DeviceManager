using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceManager.Models
{
    public class JSONDataSource
    {
        public List<float[]> DataSets { get; set; }
        public List<Device> Generators { get; set; }
    }
}
