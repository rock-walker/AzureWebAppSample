using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookStore.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coach { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}
