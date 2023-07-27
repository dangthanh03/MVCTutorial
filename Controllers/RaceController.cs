using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.InterfaceRepository;
using RunGroopWebApp.Models;
using RunGroopWebApp.Service;
using RunGroopWebApp.ViewModel;

namespace RunGroopWebApp.Controllers
{
    public class RaceController : Controller
    {
        private readonly IRaceRepository iRace;
        private readonly IPhotoService photoService;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;

        public RaceController(IRaceRepository IRace, IPhotoService photoService,IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            iRace = IRace;
            this.photoService = photoService;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> Races = await iRace.GetAllRaces();
            return View(Races);
        }
        [HttpGet]
        public async  Task<IActionResult> Detail(int id)
        {
            Race race = await iRace.GetRace(id);
            return View(race);
        }

        public ActionResult Create() {
            var curUserId = httpContextAccessor.HttpContext.User.GetUserId();
            var createRaceVm = new CreateRaceViewModel
            {
                AppUserId = curUserId
            };
            return View(createRaceVm); }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVm)
        {
            if (ModelState.IsValid)
            {
                var result = await photoService.AddPhotoAsync(raceVm.Image);
                var club = new Race
                {
                    RaceCategory = raceVm.RaceCategory, 
                    Title = raceVm.Title,
                    Description = raceVm.Description,
                    Image = result.Url.ToString(),
                    AppUserId = raceVm.AppUserId,
                    Address = new Address
                    {
                        Street = raceVm.Address.Street,
                        City = raceVm.Address.City,
                        State = raceVm.Address.State

                    }

                };
                this.iRace.Create(club);
                return RedirectToAction("Index");

            }
            else
            {

                ModelState.AddModelError("", "Photo upload failed");
     
            }
            return View(raceVm);

        }
        public async Task<IActionResult> Edit(int id)
        {
            var Race = await this.iRace.GetRace(id);
            if (Race == null)
            {
                return View("Error");
            }
            var RaceVM = mapper.Map<EditRaceViewModel>(Race);
            return View(RaceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRaceViewModel RaceVM)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", RaceVM);
            }
            var userRace = await this.iRace.GetRace(RaceVM.Id);
            if (userRace != null)
            {
                try { await photoService.DeletePhotoAsync(userRace.Image); }

                catch (Exception ex)
                {

                    ModelState.AddModelError("", "Could not delete photo");
                    return View(RaceVM);
                }
                var photoResult = await photoService.AddPhotoAsync(RaceVM.NewImage);
                RaceVM.Image = photoResult.Url.ToString();
                var race = mapper.Map<Race>(RaceVM);


                this.iRace.Update( race);
                return RedirectToAction("Index");
            }
            else
            {
                return View(RaceVM);
            }

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ClubDetails = await iRace.GetRace(id);
            if (ClubDetails == null) { return View("Error"); }
            return View(ClubDetails);

        }

        [HttpPost, ActionName("DeleteRace")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await iRace.GetRace(id);

            if (clubDetails == null) return View("Error");
           iRace.Delete(clubDetails);
            return RedirectToAction("Index");
        }

    }


}
