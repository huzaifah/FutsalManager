using FutsalManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain.Interfaces
{
    public interface ITournamentRepository
    {
        IEnumerable<Tournament> GetAll();
        Tournament GetByDate(DateTime tournamentDate);
        void Add(Tournament tournament);
    }
}
