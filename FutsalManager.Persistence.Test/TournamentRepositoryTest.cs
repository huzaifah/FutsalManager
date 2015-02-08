using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FutsalManager.Domain.Interfaces;
using FutsalManager.Persistence;
using System.IO;
using System.Collections.Generic;
using FutsalManager.Persistence.Entities;
using FutsalManager.Domain.Dtos;

namespace FutsalManager.Domain.Test
{
    [TestClass]
    public class TournamentRepositoryTest
    {
        private TournamentRepository tournamentRepository;
        Guid teamGuid = Guid.NewGuid();
        Guid playerGuid = Guid.NewGuid();
        Guid tournamentGuid = Guid.NewGuid();
        Guid matchGuid = Guid.NewGuid();

        [TestInitialize]
        public void Initialize()
        {
            tournamentRepository = new TournamentRepository(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "futsal.db"));
        }

        [TestMethod]
        public void CreateNewTables_SQLite_CreateTables()
        {            
            tournamentRepository.CreateNewTables();
        }

        [TestMethod]
        public void GetAllTournaments_SQLite_ReturnList()
        {
            var tournamentList = tournamentRepository.GetAll();

            Assert.IsInstanceOfType(tournamentList, typeof(List<TournamentDto>));
        }

        [TestMethod]
        public void GetTournament_ByDate_ReturnTournament()
        {
            var tournamentDate = new DateTime(2015, 2, 1);
            var tournament = tournamentRepository.GetByDate(tournamentDate);
        }

        [TestMethod]
        public void AddTournament_ValidTournament_SavedTournament()
        {
            var tournament = new TournamentDto
                {
                    Date = new DateTime(2015, 2, 1),
                    MaxPlayerPerTeam = 6,
                    TotalTeam = 4
                };

            tournament.Id = tournamentGuid.ToString();

            tournamentRepository.Add(tournament);
        }

        [TestMethod]
        public void AssignPlayer_NewPlayer_SavePlayerPlayerAssignments()
        {
            var playerDto = new PlayerDto
            {
                Id = playerGuid.ToString(),
                Name = "Nathan",
                TeamId = teamGuid.ToString(),
                TournamentId = tournamentGuid.ToString()
            };

            tournamentRepository.AssignPlayer(playerDto);
        }

        [TestMethod]
        public void GetPlayerByName_ReturnPlayerList()
        {
            var playerList = tournamentRepository.GetPlayersByName("Ali");
        }

        [TestMethod]
        public void AddEditTeam_NewTeam_SaveTeam()
        {
            var team = new TeamDto
            {
                Id = teamGuid.ToString(),
                Name = "Blue"
            };

            tournamentRepository.AddEditTeam(team);
        }

        [TestMethod]
        public void AssignTeam_WhiteTeam_SaveAssignment()
        {
            string teamId = "46f824db-2184-459b-ba1a-d3f633ed457a";
            var team = tournamentRepository.GetTeamById(Guid.Parse(teamId));
            tournamentRepository.AssignTeam(tournamentGuid.ToString(), team);
        }

        [TestMethod]
        public void AddEditPlayer_NewPlayer_SavePlayer()
        {
            var player = new PlayerDto
            {
                Id = playerGuid.ToString(),
                Name = "Bas"
            };

            tournamentRepository.AddEditPlayer(player);
        }

        [TestMethod]
        public void AddMatch_NewMatch_SaveMatch()
        {
            string tournamentId = "2a6a362c-cda6-45fe-b83b-da249fe1287f";
            var match = new MatchDto
            {
                HomeTeam = tournamentRepository.GetTeamByName("White"),
                AwayTeam = tournamentRepository.GetTeamByName("Blue")                
            };

            tournamentRepository.AddMatch(tournamentId, match);
        }

        /*
        IEnumerable<TournamentDto> GetAll();
        TournamentDto GetByDate(DateTime tournamentDate);
        string Add(TournamentDto tournament);
        IEnumerable<PlayerDto> GetPlayersByName(string playerName);
        PlayerDto GetPlayerById(string playerId);
        string AddEditTeam(TeamDto team);
        void AssignTeam(string tournamentId, TeamDto team);
        string AddEditPlayer(PlayerDto player);
        void AssignPlayer(PlayerDto player);
        string AddMatch(string tournamentId, MatchDto match);
        void AddMatchScore(string tournamentId, string matchId, string teamId, string playerId, string remark);
        IEnumerable<TeamDto> GetTeamsByTournament(string tournamentId);
        int GetTotalTeamsByTournament(string tournamentId);
        IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId);
        int GetTotalPlayerByTeam(string tournamentId, string teamId);
        IEnumerable<MatchDto> GetMatches(string tournamentId);
         */
    }
}
