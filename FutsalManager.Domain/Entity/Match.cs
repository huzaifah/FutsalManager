using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Match
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        public Match(Team home, Team away)
        {
            HomeTeam = home;
            AwayTeam = away;
        }
    }
}
