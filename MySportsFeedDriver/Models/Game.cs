using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySportsFeedDriver.Models
{
    public class Game
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Team AwayTeam { get; set; }
        public Team HomeTeam { get; set; }
        public string Location { get; set; }
    }
}
