using FutsalManager.Domain.Dtos;
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
        IEnumerable<TournamentDto> GetAll();
        TournamentDto GetByDate(DateTime tournamentDate);
        string Add(TournamentDto tournament);
        IEnumerable<PlayerDto> GetPlayersByName(string playerName);
        PlayerDto GetPlayerById(string playerId);
        string AddTeam(string tournamentId, Team team);
        string AddPlayer(string playerName);
        string AssignPlayer(PlayerDto player);
        string AddMatch(string tournamentId, Match match);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId);
        IEnumerable<Team> GetTeamsByTournament(string tournamentId);
        int GetTotalTeamsByTournament(string tournamentId);
        IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId);
        int GetTotalPlayerByTeam(string tournamentId, string teamId);
        IEnumerable<Match> GetMatches(string tournamentId);
    }
}
