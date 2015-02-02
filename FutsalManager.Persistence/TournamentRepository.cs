using FutsalManager.Domain.Interfaces;
using FutsalManager.Persistence.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence
{
    public class TournamentRepository
    {
        private readonly SQLiteConnection db;

        public TournamentRepository(string databasePath)
        {
            db = new SQLiteConnection(databasePath);
        }

        public void CreateNewTables()
        {
            db.CreateTable<Tournament>();
        }

        public IEnumerable<Tournament> GetAll()
        {
            var tournamentList = db.Query<Tournament>("Select * from Tournament");
            return tournamentList;            
        }

        public Tournament GetByDate(DateTime tournamentDate)
        {
            var tournament = db.Table<Tournament>().FirstOrDefault(x => x.Date == tournamentDate);
            return tournament;
        }

        public void Add(Tournament tournament)
        {
            db.Insert(tournament);
        }

        /*
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
         */
    }
}
