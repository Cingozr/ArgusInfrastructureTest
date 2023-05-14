using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MouseKeybordTest.Models
{
    public  class ForegroundAppModel
    {
        public string AppName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public TimeSpan TotalActiveTime
        {
            get
            {
                return (EndDate ?? DateTime.Now) - StartDate;
            }
        }
    }
}
