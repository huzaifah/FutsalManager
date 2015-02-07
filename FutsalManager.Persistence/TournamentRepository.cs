using FutsalManager.Domain.Dtos;
using FutsalManager.Domain.Interfaces;
using FutsalManager.Persistence.Entities;
using FutsalManager.Persistence.Helpers;
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
            db.CreateTable<Tournaments>();
            db.CreateTable<Players>();
            db.CreateTable<PlayerAssignments>();
        }

        public IEnumerable<TournamentDto> GetAll()
        {
            var tournaments = db.Query<Tournaments>("Select * from Tournament");
            return tournaments.ConvertAll(t => t.ConvertToDto());            
        }

        public TournamentDto GetByDate(DateTime tournamentDate)
        {
            var tournament = db.Table<Tournaments>().FirstOrDefault(x => x.Date == tournamentDate);
            return tournament.ConvertToDto();
        }

        public string Add(TournamentDto tournament)
        {
            tournament.Id = Guid.NewGuid().ToString();
            db.Insert(tournament.ConvertToDb(), typeof(Tournaments));
            return tournament.Id;
        }

        public IEnumerable<PlayerDto> GetPlayersByName(string playerName)
        {
            playerName = "%" + playerName + "%";
            var players = db.Table<Players>().Where(p => p.Name == playerName).ToList();
            return players.ConvertAll(player => player.ConvertToDto());
        }

        public PlayerDto GetPlayerById(string playerId)
        {
            Guid playerGuid;

            if (!Guid.TryParse(playerId, out playerGuid))
                throw new ArgumentException("Player id is invalid");
            
            var player = db.Table<Players>().Where(p => p.Id == playerGuid).SingleOrDefault();
            return player.ConvertToDto();
        }

        public PlayerDto GetPlayerById(Guid playerGuid)
        {
            var player = db.Table<Players>().Where(p => p.Id == playerGuid).SingleOrDefault();
            return player.ConvertToDto();
        }

        public void AssignPlayer(PlayerDto player)
        {
            Guid playerId, tournamentId, teamId;

            if (Guid.TryParse(player.Id, out playerId) && Guid.TryParse(player.TeamId, out teamId) && Guid.TryParse(player.TournamentId, out tournamentId))
                db.Insert(new PlayerAssignments { PlayerId = playerId, TournamentId = tournamentId, TeamId = teamId }, typeof(PlayerAssignments));
        }

        public string AddEditPlayer(PlayerDto player)
        {
            if (GetPlayerById(player.Id) == null)
            {
                player.Id = Guid.NewGuid().ToString();
                db.Insert(player.ConvertToDb(), typeof(Players));
            }
            else
            {
                db.Update(player.ConvertToDb(), typeof(Players));
            }

            return player.Id;
        }

        public IEnumerable<PlayerDto> GetPlayersByTeam(string tournamentId, string teamId)
        {
            Guid tournamentGuid, teamGuid = Guid.Empty;

            if (Guid.TryParse(tournamentId, out tournamentGuid) && Guid.TryParse(teamId, out teamGuid))
                throw new ArgumentException("Tournament id or team id is invalid");

            var teamPlayers = db.Table<PlayerAssignments>().Where(x => x.TournamentId == tournamentGuid && x.TeamId == teamGuid);
            var playerList = teamPlayers.ToList().ConvertAll(player => new PlayerDto
                {
                    Id = player.PlayerId.ToString(),
                    Name = GetPlayerById(player.PlayerId).Name,
                    TeamId = player.TeamId.ToString(),
                    TournamentId = player.TournamentId.ToString()
                });

            return playerList;
        }

        public int GetTotalPlayerByTeam(string tournamentId, string teamId)
        {
            Guid tournamentGuid, teamGuid = Guid.Empty;

            if (Guid.TryParse(tournamentId, out tournamentGuid) && Guid.TryParse(teamId, out teamGuid))
                throw new ArgumentException("Tournament id or team id is invalid");

            return db.Table<PlayerAssignments>().Where(x => x.TournamentId == tournamentGuid && x.TeamId == teamGuid).Count();
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
