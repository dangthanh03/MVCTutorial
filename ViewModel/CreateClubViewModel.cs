using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModel
{
    public class CreateClubViewModel
    {
        public string? AppUserId { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
    }
}
