using kursovOsn.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace kursovOsn.Server.Services
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDto>> GetAllTournamentsAsync();
        Task<Tournament> GetTournamentByIdAsync(int id);
        Task<Tournament> CreateTournamentAsync(Tournament tournament);
        Task<Tournament> UpdateTournamentAsync(int id, Tournament tournament);
        Task<bool> DeleteTournamentAsync(int id);
    }
}
