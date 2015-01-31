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
        Player GetPlayerById(string playerId);
        void AddTeam(string tournamentId, Team team);
        void AddPlayer(string tournamentId, string teamId, Player player);
        void AddMatch(string tournamentId, Match match);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId);
        IEnumerable<Team> GetTeamsByTournament(string tournamentId);
        int GetTotalTeamsByTournament(string tournamentId);
        IEnumerable<Player> GetPlayersByTeam(string tournamentId, string teamId);
        int GetTotalPlayerByTeam(string tournamentId, string teamId);
        IEnumerable<Match> GetMatches(string tournamentId);
    }
}
