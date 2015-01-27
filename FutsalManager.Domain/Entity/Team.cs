﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Entity
{
    public class Team
    {
        public string Name { get; set; }
        public IList<Player> Players { get; set; }

        public Team(string name)
        {
            Name = name;

            Players = new List<Player>();
        }
    }
}