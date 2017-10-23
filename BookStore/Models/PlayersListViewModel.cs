using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Models
{
    public class PlayersListViewModel
    {
        public IEnumerable<Player> Players { get; set; }
        public SelectList Teams { get; set; }
        public SelectList Positions { get; set; }
    }
}