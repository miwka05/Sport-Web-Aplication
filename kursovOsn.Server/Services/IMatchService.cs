using kursovOsn.Server.Models;

namespace kursovOsn.Server.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> GetMatchByIdAsync(int id);
        Task<Match> CreateMatchAsync(Match match);
        Task<Match> UpdateMatchAsync(int id, Match match);
        Task<bool> DeleteMatchAsync(int id);
    }
}
