using FutsalManager.Domain.Dtos;
using FutsalManager.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutsalManager.Persistence.Helpers
{
    public static class DbMapper
    {
        public static TournamentDto ConvertToDto(this Tournaments tournament)
        {
            return new TournamentDto
            {
                Id = tournament.Id.ToString(),
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static Tournaments ConvertToDb(this TournamentDto tournament)
        {
            return new Tournaments
            {
                Id = Guid.Parse(tournament.Id),
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static PlayerDto ConvertToDto(this Players player)
        {
            if (player == null)
                return default(PlayerDto);

            return new PlayerDto
            {
                Id = player.Id.ToString(),
                Name = player.Name
                //TeamId = player.TeamId,
                //TournamentId = player.TournamentId
            };
        }

        public static Players ConvertToDb(this PlayerDto player)
        {
            return new Players
            {
                Id = Guid.Parse(player.Id),
                Name = player.Name
                //TeamId = player.TeamId,
                //TournamentId = player.TournamentId
            };
        }
    }
}
