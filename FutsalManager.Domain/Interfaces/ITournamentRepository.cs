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
        Player GetPlayerByName(string playerName);
        void AddTeam(string tournamentId, Team team);
        void AddPlayer(string tournamentId, string teamId, Player player);
        void AddMatch(string tournamentId, string homeTeamId, string awayTeamId);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId);
    }
}
