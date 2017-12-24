using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySportsFeedDriver.Models
{
    public class PlayerGameLog
    {
        public Game GameInfo { get; set; }
        public Player PlayerInfo { get; set; }
        public PlayerGameStats PlayerGameStats { get; set; }
    }
}
