using RunGroopWebApp.Models;

namespace RunGroopWebApp.InterfaceRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        bool Add(AppUser user);
        bool Delete(AppUser user);
        bool Save();
        bool Update(AppUser user);
    }
}
