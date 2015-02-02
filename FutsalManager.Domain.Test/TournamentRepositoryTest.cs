using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FutsalManager.Domain.Interfaces;
using FutsalManager.Persistence;
using System.IO;
using System.Collections.Generic;
using FutsalManager.Persistence.Entities;

namespace FutsalManager.Domain.Test
{
    [TestClass]
    public class TournamentRepositoryTest
    {
        private TournamentRepository tournamentRepository;
        
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

            Assert.IsInstanceOfType(tournamentList, typeof(List<Tournament>));
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
            var tournament = new Tournament
                {
                    Date = new DateTime(2015, 2, 1),
                    MaxPlayerPerTeam = 6,
                    TotalTeam = 4
                };

            tournament.Id = Guid.NewGuid().ToString();

            tournamentRepository.Add(tournament);
        }
    }
}
