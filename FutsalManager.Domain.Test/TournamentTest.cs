using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using FutsalManager.Domain.Interfaces;
using FakeItEasy;
using FutsalManager.Domain.Enum;

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
                    Id = "1",
                    Date = new DateTime(2015, 1, 1),
                    MaxPlayerPerTeam = 6,
                    TotalTeam = 4                    
                });

            var blue = new Team { Name = "Blue", Id="4" };
            var white = new Team("White");

            A.CallTo(() => tournamentRepo.GetTeamsByTournament("1"))
                .Returns(new List<Team>
                {
                    blue,
                    white
                });

            A.CallTo(() => tournamentRepo.GetTeamsByTournament("2"))
                .Returns(new List<Team>
                {
                    blue,
                    white
                });

            A.CallTo(() => tournamentRepo.GetTotalTeamsByTournament("1"))
                .Returns(5);

            A.CallTo(() => tournamentRepo.GetTotalTeamsByTournament("2"))
                .Returns(3);

            A.CallTo(() => tournamentRepo.GetMatches("2"))
                .Returns(new List<Match>
                {
                    new Match(blue, white)
                });

            A.CallTo(() => tournamentRepo.GetPlayersByTeam("2", "4"))
                .Returns(new List<Player>
                {
                    new Player { Id = "3", Name = "Ali" },
                    new Player { Id = "4", Name = "Rafiq" },
                    new Player { Id = "5", Name = "Nathan" },
                    new Player { Id = "6", Name = "Jon" },
                    new Player { Id = "7", Name = "Bass" },
                    new Player { Id = "8", Name = "Geoff" }
                });

            A.CallTo(() => tournamentRepo.GetPlayerById("3"))
                .Returns(new Player 
                {
                    Id = "3",
                    Name = "Ali"
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
            tournament.Id = "1";
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
            tournament.Id = "2";

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AddTeam(tournament, new Team("Black"));
            tournamentService.AddTeam(tournament, new Team("Red"));
            tournamentService.AddTeam(tournament, new Team("White"));            
        }

        [TestMethod]
        public void AssignPlayer_WithinLimit_PlayerAdded()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournament.Id = "2";

            var team = tournamentService.RetrieveTeams(tournament).FirstOrDefault(x => x.Name == "Blue");

            tournamentService.AssignPlayer(tournament, team, new Player("Ali"));
            tournamentService.AssignPlayer(tournament, team, new Player("Rafiq"));
            tournamentService.AssignPlayer(tournament, team, new Player("Nathan"));
            tournamentService.AssignPlayer(tournament, team, new Player("Jon"));
            tournamentService.AssignPlayer(tournament, team, new Player("Bass"));
            tournamentService.AssignPlayer(tournament, team, new Player("Geoff"));

            var playerList = tournamentService.RetrievePlayers(tournament, team);
            Assert.IsTrue(playerList.Count() == 6);
        }

        [TestMethod, ExpectedException(typeof(TeamNotFoundException))]
        public void AssignPlayer_TeamNotFound_ThrowsException()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournamentService.CreateTournament(tournament);

            tournamentService.AddTeam(tournament, new Team("Blue"));
            tournamentService.AssignPlayer(tournament, new Team("Grey"), new Player("Ali"));
        }

        [TestMethod]
        public void CreateMatch_ValidTeam_MatchCreated()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournament.Id = "1";
            var blue = tournamentService.RetrieveTeams(tournament).FirstOrDefault(x => x.Name == "Blue");
            var white = tournamentService.RetrieveTeams(tournament).FirstOrDefault(x => x.Name == "White");
            
            tournamentService.AddMatch(tournament, blue, white);
        }

        [TestMethod, ExpectedException(typeof(TeamNotFoundException))]
        public void CreateMatch_TeamNotFound_ThrowsException()
        {
            var tournament = new Tournament(new DateTime(2015, 5, 5), 4, 6);
            tournamentService.CreateTournament(tournament);

            var blue = new Team("Blue");
            tournamentService.AddTeam(tournament, blue);

            tournamentService.AddMatch(tournament, blue, new Team("White"));
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

        [TestMethod]
        public void AddScore_ValidMatch_SaveScore()
        {
            var tournamentDate = new DateTime(2015, 1, 1);
            var tournament = tournamentService.RetrieveTournamentByDate(tournamentDate);
            tournament.Id = "2";

            var matches = tournamentService.RetrieveMatches(tournament);
            var match = matches.FirstOrDefault(x => x.HomeTeam.Name == "Blue" && x.AwayTeam.Name == "White");

            tournamentService.AddScore(tournament, match, TeamSide.Home, "3");
        }
    }
}
