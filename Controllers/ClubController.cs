using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModel;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepostiroy club;
        private readonly IPhotoService photoService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClubController(IClubRepostiroy club, IPhotoService photoService, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            this.club = club;
            this.photoService = photoService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await club.GetAllClub();
            return View(clubs);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            Club club = await this.club.GetClub(id);

            return View(club);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserId = httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel { AppUserId = curUserId };
            return View(createClubViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVm)
        {
            if (ModelState.IsValid)
            {
                var result = await photoService.AddPhotoAsync(clubVm.Image);
                var club = new Club
                {
                    ClubCategory = clubVm.ClubCategory,
                    Title = clubVm.Title,
                    Description = clubVm.Description,
                    Image = result.Url.ToString(),
                    AppUserId=clubVm.AppUserId,
                    Address = new Address
                    {
                        Street = clubVm.Address.Street,
                        City = clubVm.Address.City,
                        State = clubVm.Address.State

                    }

                };
                this.club.Create(club);
                return RedirectToAction("Index");

            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");

            }
            return View(clubVm);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await this.club.GetClub(id);
            if (club == null)
            {
                return View("Error");
            }
            var clubVM = mapper.Map<EditClubViewModel>(club);
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( EditClubViewModel clubVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }
            var userClub = await this.club.GetClub(clubVM.Id);
            if (userClub != null)
            {
                try { await photoService.DeletePhotoAsync(userClub.Image); }

                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await photoService.AddPhotoAsync(clubVM.NewImage);
                clubVM.Image = photoResult.Url.ToString();
                var club1 = mapper.Map<Club>(clubVM);
               
                
                this.club.Update(club1);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ClubDetails = await club.GetClub(id);
            if (ClubDetails == null) { return View("Error"); }
            return View(ClubDetails);

        }

        [HttpPost,ActionName("DeleteClub")]
      public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await club.GetClub(id);

            if (clubDetails == null) return View("Error");
            club.Delete(clubDetails);
            return RedirectToAction("Index");
        }
    
    }
}
