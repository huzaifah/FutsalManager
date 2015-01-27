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
        }

        public void AssignPlayer(Tournament tournament, string teamName, Player player)
        {
            var team = tournament.TeamList.SingleOrDefault(x => String.Compare(x.Name, teamName, true) == 0);

            if (team == null)
                throw new TeamNotFoundException();

            team.Players.Add(player);
        }

        public void AddMatch(Tournament tournament, string homeTeam, string awayTeam)
        {
            var home = tournament.TeamList.SingleOrDefault(x => String.Compare(homeTeam, x.Name) == 0);
            var away = tournament.TeamList.SingleOrDefault(x => String.Compare(awayTeam, x.Name) == 0);

            if (home == null || away == null)
                throw new TeamNotFoundException();

            var match = new Match(home, away);

            tournament.AddMatch(match);
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
    }
}
