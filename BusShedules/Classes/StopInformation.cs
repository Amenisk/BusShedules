using BusShedules.ADOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BusShedules.Classes
{
    public class StopInformation
    {
        public int StopId { get; set; }

        public string StopName { get; set; }

        public string Time { get; set; }

        public string Highlighting { get; set; }

        public Brush Color { get; set; }

        public StopInformation(Stop stop, DateTime time)
        {
            StopId = stop.StopId;
            StopName = stop.StopName.Name;
            Time = time.TimeOfDay.ToString();
            if (time < DateTime.Now)
            {
                Color = Brushes.Gray;
            }
            else
            {
                Color = Brushes.Black;
            }
        }
    }
}
