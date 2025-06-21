using kursovOsn.Server.Data;
using kursovOsn.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovOsn.Server.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext _context;

        public TournamentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync()
        {
            return await _context.Tournaments
                .Include(t => t.Sport)
                .Include(t => t.Format)
                .Select(t=> new TournamentDto
                {
                    ID = t.ID,
                    Name = t.Name,
                    SportName = t.Sport.Name,
                    Start = t.Start,
                    End = t.End,
                    Ban = t.ban,
                    Solo = t.solo,
                    reasonBan = t.reasonBan
                })
                .ToListAsync();
        }

        public async Task<Tournament> GetTournamentByIdAsync(int id)
        {
            return await _context.Tournaments
                .Include(t => t.Sport)
                .Include(t => t.Format)
                .Include(t => t.Creator)  // Добавляем загрузку данных о создателе
                .FirstOrDefaultAsync(t => t.ID == id);
        }

        public async Task<Tournament> CreateTournamentAsync(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
            await _context.SaveChangesAsync();
            return tournament;
        }

        public async Task<Tournament> UpdateTournamentAsync(int id, Tournament tournament)
        {
            if (id != tournament.ID)
            {
                return null;
            }

            _context.Entry(tournament).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tournament;
        }

        public async Task<bool> DeleteTournamentAsync(int id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return false;
            }

            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
