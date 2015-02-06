using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Enum;
using FutsalManager.Domain.Exceptions;
using FutsalManager.Domain.Helpers;
using FutsalManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Domain
{
    public class TournamentService
    {
        private readonly ITournamentRepository tournamentRepo;

        public TournamentService(ITournamentRepository repo)
        {
            tournamentRepo = repo;
        }

        public void CreateTournament(Tournament tournament)
        {
            tournamentRepo.Add(tournament.ConvertToDto());
        }

        public void AddTeam(Tournament tournament, Team team)
        {
            var teamCount = tournamentRepo.GetTotalTeamsByTournament(tournament.Id);

            if (teamCount >= tournament.TotalTeam)
                throw new ExceedTotalTeamsException();
                        
            tournamentRepo.AddTeam(tournament.Id, team);
        }

        public void AssignPlayer(Tournament tournament, Team team, Player player)
        {
            var teamList = RetrieveTeams(tournament);

            if (!teamList.Contains(team))
                throw new TeamNotFoundException();

            tournamentRepo.AddPlayer(tournament.Id, team.Id, player.ConvertToDto());
        }

        public void AddMatch(Tournament tournament, Team home, Team away)
        {
            var teamList = RetrieveTeams(tournament);

            if (!teamList.Contains(home) || !teamList.Contains(away))
                throw new TeamNotFoundException();

            var match = new Match(home, away);
                        
            tournamentRepo.AddMatch(tournament.Id, match);
        }

        public void AddMatch(Tournament tournament, Match match)
        {
            var teamList = RetrieveTeams(tournament);

            if (teamList.FirstOrDefault(x => x.Name == match.HomeTeam.Name)!=null
                || 
                teamList.FirstOrDefault(x => x.Name == match.AwayTeam.Name)!=null)
                throw new TeamNotFoundException();

            tournamentRepo.AddMatch(tournament.Id, match);
        }

        public IEnumerable<Tournament> RetrieveTournament()
        {
            var tournamentList = tournamentRepo.GetAll();
            var tournaments = tournamentList.ToList().ConvertAll(t => t.ConvertToEntity());
            return tournaments;
        }

        public Tournament RetrieveTournamentByDate(DateTime tournamentDate)
        {
            var tournament = tournamentRepo.GetByDate(tournamentDate);
            return tournament.ConvertToEntity();
        }

        public void AddScore(Tournament tournament, Match match, TeamSide scoredSide, string playerId, string remark = "")
        {
            var scoredTeam = scoredSide == TeamSide.Home ? match.HomeTeam : match.AwayTeam;

            var playerList = tournamentRepo.GetPlayersByTeam(tournament.Id, scoredTeam.Id);
            var player = playerList.SingleOrDefault(x => x.Id == playerId);

            if (player == null)
                if (String.IsNullOrEmpty(remark))
                    throw new PlayerNotFoundException();
                else
                    player = tournamentRepo.GetPlayerById(playerId);

            tournamentRepo.AddMatchScore(tournament.Id, match.Id, scoredTeam.Id, player.Id);
        }

        public IEnumerable<Player> RetrievePlayers(Tournament tournament, Team team)
        {
            var players = tournamentRepo.GetPlayersByTeam(tournament.Id, team.Id);
            return players.ToList().ConvertAll(player => player.ConvertToEntity());
        }

        public IEnumerable<Match> RetrieveMatches(Tournament tournament)
        {
            return tournamentRepo.GetMatches(tournament.Id);
        }

        public IEnumerable<Team> RetrieveTeams(Tournament tournament)
        {
            return tournamentRepo.GetTeamsByTournament(tournament.Id);
        }

    }
}
