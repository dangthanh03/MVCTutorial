using RunGroopWebApp.Models;

namespace RunGroopWebApp
{
    public interface IClubRepostiroy
    {
        Task<IEnumerable<Club>> GetAllClub();
        Task<Club> GetClub(int id);
        Task<IEnumerable<Club>> GetClubByCity(string city);

        public bool Create(Club club);
        public bool Update(Club newclub);
        public bool Delete(Club club);
        public bool Save();
    }
}
