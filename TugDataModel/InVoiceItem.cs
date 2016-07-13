using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TugDataModel
{
    public class InVoiceItem
    {
        public int SchedulerID { get; set; } 

        public int ItemID { get; set; } 

        public double UnitPrice { get; set; } 

        public string Currency { get; set; } 
    }
}
