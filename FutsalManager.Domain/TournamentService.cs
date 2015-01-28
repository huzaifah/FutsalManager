using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Exceptions;
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
            tournamentRepo.Add(tournament);
        }

        public void AddTeam(Tournament tournament, Team team)
        {
            if (tournament.TeamList.Count >= tournament.TotalTeam)
                throw new ExceedTotalTeamsException();

            tournament.AddTeam(team);
            tournamentRepo.AddTeam(tournament.Id, team);
        }

        public void AssignPlayer(Tournament tournament, Team team, Player player)
        {
            if (!tournament.TeamList.Contains(team))
                throw new TeamNotFoundException();

            team.Players.Add(player);
            tournamentRepo.AddPlayer(tournament.Id, team.Id, player);
        }

        public void AddMatch(Tournament tournament, Team home, Team away)
        {
            if (!tournament.TeamList.Contains(home) || !tournament.TeamList.Contains(away))
                throw new TeamNotFoundException();

            var match = new Match(home, away);

            tournament.AddMatch(match);
            tournamentRepo.AddMatch(tournament.Id, home.Id, away.Id);
        }

        public IEnumerable<Tournament> RetrieveTournament()
        {
            var tournamentList = tournamentRepo.GetAll();
            return tournamentList;
        }

        public Tournament RetrieveTournamentByDate(DateTime tournamentDate)
        {
            var tournament = tournamentRepo.GetByDate(tournamentDate);
            return tournament;
        }

        public void AddScore(Tournament tournament, Match match, Team scoredTeam, string scorerName, string remark = "")
        {
            var player = scoredTeam.Players.SingleOrDefault(x => x.Name == scorerName);

            if (player == null)
                if (String.IsNullOrEmpty(remark))
                    throw new PlayerNotFoundException();
                else
                    player = tournamentRepo.GetPlayerByName(scorerName);

            match.AddScore(scoredTeam, player, remark);
            tournamentRepo.AddMatchScore(tournament.Id, match.Id, scoredTeam.Id, player.Id);
        }

    }
}
