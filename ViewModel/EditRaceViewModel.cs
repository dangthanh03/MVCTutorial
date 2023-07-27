using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace RunGroopWebApp.ViewModel
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile NewImage { get; set; }
        public string? Image { get; set; }
        [ForeignKey("Address")]
        [HiddenInput(DisplayValue = false)]
        public int? AddressId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Address? Address { get; set; }
        public RaceCategory RaceCategory { get; set; }


    }

}
