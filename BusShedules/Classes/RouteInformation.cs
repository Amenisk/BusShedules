using BusShedules.ADOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusShedules.Classes
{
    public class RouteInformation
    {
        public int RouteId { get; set; }
        public string Name { get; set; }
        public string TimePeriod { get; set; }
        public string Date { get; set; }
        public string StopCount { get; set; }
        public string BusNumber { get; set; }
        public DateTime BeginningDateTime { get; set; }
        public DateTime EndingDateTime { get; set; }
        public byte[] Map { get; set; }
        public string MapLink { get; set; }
        public List<StopInformation> StopsList { get; set; } = new List<StopInformation>();

        public RouteInformation(Route route, DateTime beginningTime)
        {
            RouteId = route.RouteId;
            Name = route.Name;
            BeginningDateTime = beginningTime;
            Map = route.Map;
            MapLink = route.MapLink;
            BusNumber = route.BusNumber.Number.ToString();
            var dt = beginningTime.Date.ToString().Split(' ');
            Date = dt[0];
            StopCount = route.RouteStop.Count.ToString();

            foreach (var s in App.Connection.RouteStop.Where(x => x.RouteId == RouteId))
            {
                var stop = App.Connection.Stop.FirstOrDefault(x => x.StopId == s.StopId);
                beginningTime = beginningTime.AddMinutes(stop.ArrivalTime);
                StopsList.Add(new StopInformation(stop, beginningTime));
            }

            TimePeriod = $"{StopsList.First().Time} : {StopsList.Last().Time}";
            EndingDateTime = beginningTime;
        }

        public RouteInformation(Route route)
        {
            RouteId = route.RouteId;
            Name = route.Name;
            Map = route.Map;
            MapLink = route.MapLink;
            BusNumber = route.BusNumber.Number.ToString();
            StopCount = route.RouteStop.Count.ToString();
        }

    }
}
