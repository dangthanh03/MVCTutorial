using RunGroopWebApp.Models;

namespace RunGroopWebApp.InterfaceRepository
{
    public interface IDashboardRepository
    {
        Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        public bool Update(AppUser user);
    }
}
