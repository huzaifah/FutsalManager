using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using FutsalManager.Domain.Interfaces;
using FakeItEasy;

namespace FutsalManager.Domain.Test
{
    [TestClass]
    public class TournamentTest
    {
        TournamentService tournamentService;
        ITournamentRepository tournamentRepo;

        [TestInitialize]
        public void Initialize()
        {
            tournamentRepo = A.Fake<ITournamentRepository>();

            A.CallTo(() => tournamentRepo.GetAll())
                .Returns(new List<Tournament> { 
                    new Tournament(DateTime.Now, 4, 6),
                    new Tournament(DateTime.Now.AddDays(2), 5, 10)
                });

            A.CallTo(() => tournamentRepo.GetByDate(new DateTime(2015, 1, 1)))
                .Returns(new Tournament
                {
                    Date = new DateTime(2015, 1, 1),
                    MaxPlayerPerTeam = 6,
                    TotalTeam = 4                    
                });

            tournamentService = new TournamentService(tournamentRepo);
        }

        [TestMethod]
        public void CreateTournament_ValidTournament_TournamentCreated()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 8);
            tournamentService.CreateTournament(tournament);
        }

        [TestMethod, ExpectedException(typeof(ExceedTotalTeamsException))]
        public void AddTeam_ExceedLimit_ThrowsException()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 8);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AddTeam(tournament, new Team("Black"));
            tournamentService.AddTeam(tournament, new Team("Red"));
            tournamentService.AddTeam(tournament, new Team("White"));
            tournamentService.AddTeam(tournament, new Team("Yellow"));
        }

        [TestMethod]
        public void AddTeam_WithinLimit_TeamAdded()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 8);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AddTeam(tournament, new Team("Black"));
            tournamentService.AddTeam(tournament, new Team("Red"));
            tournamentService.AddTeam(tournament, new Team("White"));            
        }

        [TestMethod]
        public void AssignPlayer_WithinLimit_PlayerAdded()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AssignPlayer(tournament, "Blue", new Player("Ali"));
            tournamentService.AssignPlayer(tournament, "Blue", new Player("Rafiq"));
            tournamentService.AssignPlayer(tournament, "Blue", new Player("Nathan"));
            tournamentService.AssignPlayer(tournament, "Blue", new Player("Jon"));
            tournamentService.AssignPlayer(tournament, "Blue", new Player("Bass"));
            tournamentService.AssignPlayer(tournament, "Blue", new Player("Geoff"));

            Assert.IsTrue(tournament.TeamList[0].Players.Count == 6);
        }

        [TestMethod, ExpectedException(typeof(TeamNotFoundException))]
        public void AssignPlayer_TeamNotFound_ThrowsException()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AssignPlayer(tournament, "Grey", new Player("Ali"));
        }

        [TestMethod]
        public void CreateMatch_ValidTeam_MatchCreated()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AddTeam(tournament, new Team("White"));

            tournamentService.AddMatch(tournament, "Blue", "White");
        }

        [TestMethod, ExpectedException(typeof(TeamNotFoundException))]
        public void CreateMatch_TeamNotFound_ThrowsException()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));

            tournamentService.AddMatch(tournament, "Blue", "White");
        }

        [TestMethod]
        public void RetrieveTournament_AllTournament()
        {
            var tournamentList = tournamentService.RetrieveTournament();

            Assert.IsInstanceOfType(tournamentList, typeof(IEnumerable<Tournament>));
            Assert.IsTrue(tournamentList.Count() == 2);
        }

        [TestMethod]
        public void RetrieveSingleTournament_ByDate_ReturnTournament()
        {
            var tournamentDate = new DateTime(2015, 1, 1);
            var tournament = tournamentService.RetrieveTournamentByDate(tournamentDate);
            Assert.IsInstanceOfType(tournament, typeof(Tournament));
            Assert.IsTrue(tournament.Date == tournamentDate);
        }

    }
}
