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
        public static TournamentDto ConvertToDto(this Tournament tournament)
        {
            return new TournamentDto
            {
                Id = tournament.Id,
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }

        public static Tournament ConvertToDb(this TournamentDto tournament)
        {
            return new Tournament
            {
                Id = tournament.Id,
                Date = tournament.Date,
                TotalTeam = tournament.TotalTeam,
                MaxPlayerPerTeam = tournament.MaxPlayerPerTeam
            };
        }
    }
}
