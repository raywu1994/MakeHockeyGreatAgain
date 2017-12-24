using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySportsFeedDriver.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int JerseyNumber { get; set; }
        public string Position { get; set; }
        public Team Team { get; set; }
    }
}
