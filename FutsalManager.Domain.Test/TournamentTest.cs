using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FutsalManager.Domain.Entity;
using FutsalManager.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;
using FutsalManager.Domain.Interfaces;
using FakeItEasy;
using FutsalManager.Domain.Enum;
using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Helpers;

namespace FutsalManager.Domain.Test
{
    [TestClass]
    public class TournamentTest
    {
        TournamentService tournamentService;
        ITournamentRepository tournamentRepo;
        Tournament tournamentA;
        Tournament tournamentB;
        Team blue;
        Team white;
        Match match;

        [TestInitialize]
        public void Initialize()
        {
            string tournamentIdA = Guid.NewGuid().ToString();
            string tournamentIdB = Guid.NewGuid().ToString();
            string teamBlueId = Guid.NewGuid().ToString();
            string teamWhiteId = Guid.NewGuid().ToString();
            string matchId = Guid.NewGuid().ToString();

            tournamentA = new Tournament
            {
                Date = new DateTime(2015, 5, 5),
                MaxPlayerPerTeam = 8,
                TotalTeam = 4,
                Id = tournamentIdA
            };

            tournamentB = new Tournament
            {
                Date = new DateTime(2015, 5, 5),
                MaxPlayerPerTeam = 8,
                TotalTeam = 4,
                Id = tournamentIdB
            };

            blue = new Team { Name = "Blue", Id = teamBlueId };
            white = new Team { Name = "White", Id = teamWhiteId };

            match = new Match { HomeTeam = blue, AwayTeam = white, TournamentId = tournamentIdA };

            tournamentRepo = A.Fake<ITournamentRepository>();

            A.CallTo(() => tournamentRepo.GetAll())
                .Returns(new List<TournamentDto> { 
                    new TournamentDto
                    {
                        Date = DateTime.Now, 
                        MaxPlayerPerTeam = 6,
                        TotalTeam = 4
                    },
                    new TournamentDto
                    {
                        Date = DateTime.Now.AddDays(2),
                        TotalTeam = 5,
                        MaxPlayerPerTeam = 10
                    }
                });

            A.CallTo(() => tournamentRepo.GetByDate(new DateTime(2015, 1, 1)))
                .Returns(new TournamentDto
                {
                    Id = "1",
                    Date = new DateTime(2015, 1, 1),
                    MaxPlayerPerTeam = 6,
                    TotalTeam = 4                    
                });



            A.CallTo(() => tournamentRepo.GetTeamsByTournament(tournamentIdA))
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

            A.CallTo(() => tournamentRepo.GetTotalTeamsByTournament(tournamentIdA))
                .Returns(5);

            A.CallTo(() => tournamentRepo.GetTotalTeamsByTournament("2"))
                .Returns(3);

            A.CallTo(() => tournamentRepo.GetMatches(tournamentIdA))
                .Returns(new List<Match>
                {
                    match
                });

            A.CallTo(() => tournamentRepo.GetPlayersByTeam(tournamentIdA, teamBlueId))
                .Returns(new List<PlayerDto>
                {
                    new PlayerDto { Id = "3", Name = "Ali" },
                    new PlayerDto { Id = "4", Name = "Rafiq" },
                    new PlayerDto { Id = "5", Name = "Nathan" },
                    new PlayerDto { Id = "6", Name = "Jon" },
                    new PlayerDto { Id = "7", Name = "Bass" },
                    new PlayerDto { Id = "8", Name = "Geoff" }
                });

            A.CallTo(() => tournamentRepo.GetPlayerById("3"))
                .Returns(new PlayerDto 
                {
                    Id = "3",
                    Name = "Ali"
                });

            

            A.CallTo(() => tournamentRepo.Add(tournamentA.ConvertToDto()))
                .Returns<string>(tournamentIdA);

            tournamentService = new TournamentService(tournamentRepo);
        }

        [TestMethod]
        public void CreateTournament_ValidTournament_TournamentCreated()
        {
            tournamentService.CreateTournament(tournamentA);
        }

        [TestMethod, ExpectedException(typeof(ExceedTotalTeamsException))]
        public void AddTeam_ExceedLimit_ThrowsException()
        {
            tournamentService.AddTeam(tournamentA, new Team("Blue"));
            tournamentService.AddTeam(tournamentA, new Team("Black"));
            tournamentService.AddTeam(tournamentA, new Team("Red"));
            tournamentService.AddTeam(tournamentA, new Team("White"));
            tournamentService.AddTeam(tournamentA, new Team("Yellow"));
        }

        [TestMethod]
        public void AddTeam_WithinLimit_TeamAdded()
        {
            tournamentService.AddTeam(tournamentB, new Team("Blue"));
            tournamentService.AddTeam(tournamentB, new Team("Black"));
            tournamentService.AddTeam(tournamentB, new Team("Red"));
            tournamentService.AddTeam(tournamentB, new Team("White"));            
        }

        [TestMethod]
        public void AssignPlayer_WithinLimit_PlayerAdded()
        {
            tournamentService.AssignPlayer(tournamentA, blue, new Player("Ali", blue.Id, tournamentA.Id));
            tournamentService.AssignPlayer(tournamentA, blue, new Player("Rafiq", blue.Id, tournamentA.Id));
            tournamentService.AssignPlayer(tournamentA, blue, new Player("Nathan", blue.Id, tournamentA.Id));
            tournamentService.AssignPlayer(tournamentA, blue, new Player("Jon", blue.Id, tournamentA.Id));
            tournamentService.AssignPlayer(tournamentA, blue, new Player("Bass", blue.Id, tournamentA.Id));
            tournamentService.AssignPlayer(tournamentA, blue, new Player("Geoff", blue.Id, tournamentA.Id));

            var playerList = tournamentService.RetrievePlayers(tournamentA, blue);
            Assert.IsTrue(playerList.Count() == 6);
        }

        [TestMethod, ExpectedException(typeof(TeamNotFoundException))]
        public void AssignPlayer_TeamNotFound_ThrowsException()
        {
            tournamentService.AssignPlayer(tournamentA, new Team("Grey"), new Player("Ali", Guid.NewGuid().ToString(), tournamentA.Id));
        }

        [TestMethod]
        public void CreateMatch_ValidTeam_MatchCreated()
        {
            tournamentService.AddMatch(tournamentA, blue, white);
        }

        [TestMethod, ExpectedException(typeof(TeamNotFoundException))]
        public void CreateMatch_TeamNotFound_ThrowsException()
        {            
            tournamentService.AddMatch(tournamentA, blue, new Team("White"));
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
            tournamentService.AddScore(tournamentA, match, TeamSide.Home, "3");
        }
    }
}
