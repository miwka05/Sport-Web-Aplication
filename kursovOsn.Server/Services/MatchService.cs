using kursovOsn.Server.Data;
using Microsoft.EntityFrameworkCore;
using kursovOsn.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kursovOsn.Server.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext _context;

        public MatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _context.Matches.Include(m => m.Team1_ID)
                                          .Include(m => m.Team2_ID)
                                          .ToListAsync();
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            return await _context.Matches.Include(m => m.Team1_ID)
                                          .Include(m => m.Team2_ID)
                                          .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Match> CreateMatchAsync(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<Match> UpdateMatchAsync(int id, Match match)
        {
            if (id != match.Id)
            {
                return null;
            }

            _context.Entry(match).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return false;
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
