using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TeamId { get; set; }
        public string TournamentId { get; set; }

        public Player()
        {

        }

        public Player(string name)
        {
            Name = name;
        }
    }
}
