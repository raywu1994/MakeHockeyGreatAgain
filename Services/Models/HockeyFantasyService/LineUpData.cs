using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.HockeyFantasyService
{
    public class LineUpData
    {
        public string Position { get; set; }
        public string Name { get; set; }
        public string Tier { get; set; }
        public string GameInfo { get; set; }
        public string AvgPointsPerGame { get; set; }
        public string Team { get; set; }
    }
}
