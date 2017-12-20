using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

using Microsoft.Extensions.Logging;
using WTracking.Data;
using WTracking.Models;
using WTracking.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WTracking.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApiFetcher _apiFetcher;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;
        private readonly IHostingEnvironment _environment;

        public ProfileController(
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IApiFetcher apiFetcher,
            IMemoryCache cache,
            ILogger<ProfileController> logger,
            IHostingEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _apiFetcher = apiFetcher;
            _cache = cache;
            _logger = logger;
            _environment = environment;
        }
        
        public async Task<IActionResult> Index(string searchString, string uniqueId) // why you can't use just search string?
        {
            
            var userList = from u in _context.Profile // better use AsQueryable or ToList
                           select u;

            var currUser = await _userManager.GetUserAsync(User);

            if (User.Identity.IsAuthenticated)
            {

                var profile = _context.Profile.SingleOrDefault(x => x.Id == currUser.ProfileId);

                string usrProgress = (profile.TodayStepCount >= profile.AverageStepCountForToday) ? "You're doing great!" : $"For today you should walk '{profile.AverageStepCountForToday - profile.TodayStepCount}' more steps.";
                ViewBag.UsrProgress = usrProgress; // you don't use this variable anywhere, put the value in place 

                if (!String.IsNullOrEmpty(uniqueId))
                {
                    var user = await _context.Profile.SingleOrDefaultAsync(x => x.User.UserName == uniqueId);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    return RedirectToAction(nameof(Details), user);
                }


                if (!String.IsNullOrEmpty(searchString))
                {
                    userList = userList.Where((s) => s.DisplayName == searchString);
                    // why you don't add null behavior
                }
            }

            return View(await userList.ToListAsync());
        }

        public IActionResult Details(Profile profile)
        {
            var currUser = _userManager.GetUserAsync(User);
            if (currUser == null)
            {
                return NotFound();
            } // maybe better use User.Identity.IsAuthenticated
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FetchApi(string provider, string returnUrl = null)
        {
            var currUser = await _userManager.GetUserAsync(User);
            var currProfile = await _context.Profile.SingleOrDefaultAsync(x => x.Id == currUser.ProfileId);
            if (DateTimeOffset.Now.ToUnixTimeMilliseconds() >= new DateTimeOffset(currProfile.LastFetchTime).ToUnixTimeMilliseconds() + 604800000) // Make sure, that google fit will fetch one time per week
            {

                var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Profile", new { returnUrl });
                var authProperties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
                return Challenge(authProperties, provider);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(
            string returnUrl = null, string remoteError = null)
        {

            var info = await _signInManager.GetExternalLoginInfoAsync();
            var user = await _userManager.GetUserAsync(User);
            if (info == null)
            {
                return RedirectToAction("Edit");
            }
            if (info != null)
            {
                
                var token = info.AuthenticationTokens.Single(x => x.Name == "access_token").Value;


                var lastWeekInUnix = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).ToUnixTimeMilliseconds() - 604800000;
                var todayInUnix = new DateTimeOffset(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0)).ToUnixTimeMilliseconds();
                var weekProgressJson = @"
                    {
                      'aggregateBy': [{
                      'dataTypeName': 'com.google.step_count.delta',
                      'dataSourceId': 'derived:com.google.step_count.delta:com.google.android.gms:estimated_steps'
                      }],
                      'bucketByTime': { 'durationMillis': 86400000 },
                      'startTimeMillis': " + lastWeekInUnix + @",
                      'endTimeMillis': " + DateTimeOffset.Now.ToUnixTimeMilliseconds() + @"
                    }"; // better use Json.NET


                var todaysProgressJson = @"
                    {
                      'aggregateBy': [{
                      'dataTypeName': 'com.google.step_count.delta',
                      'dataSourceId': 'derived:com.google.step_count.delta:com.google.android.gms:estimated_steps'
                      }],
                      'bucketByTime': { 'durationMillis': 86400000 },
                      'startTimeMillis': " + todayInUnix + @",
                      'endTimeMillis': " + DateTimeOffset.Now.ToUnixTimeMilliseconds() + @"
                    }";


                string content = await GoogleFitApiPostAggregate(token, weekProgressJson);
                string todayProgress = await GoogleFitApiPostAggregate(token, todaysProgressJson);

                dynamic jsonContent = JsonConvert.DeserializeObject(content); // better create special DAO for this
                dynamic todaysProgressJsonDeserialized = JsonConvert.DeserializeObject(todayProgress);

                var profile = await _context.Profile.SingleOrDefaultAsync(m => m.Id == user.ProfileId);

                int i = 0;
                try
                {
                    foreach (var item in jsonContent.bucket)
                    {
                        try // why you use try inside try?
                        {
                            if (i == 0) // use switch - case
                                profile.FirstDayProgress += (int)item.dataset[0].point[0].value[0].intVal;
                            else if (i == 1)
                                profile.SecondDayProgress += (int)item.dataset[0].point[0].value[0].intVal;
                            else if (i == 2)
                                profile.ThirdDayProgress += (int)item.dataset[0].point[0].value[0].intVal;
                            else if (i == 3)
                                profile.FourthDayProgress += (int)item.dataset[0].point[0].value[0].intVal;
                            else if (i == 4)
                                profile.FifthDayProgress += (int)item.dataset[0].point[0].value[0].intVal;
                            else if (i == 5)
                                profile.SixthDayProgress += (int)item.dataset[0].point[0].value[0].intVal;
                            else if (i == 6)
                                profile.SeventhDayProgress += (int)item.dataset[0].point[0].value[0].intVal;

                        }
                        catch
                        {
                            if (i == 0)
                                profile.FirstDayProgress = 0;
                            else if (i == 1)
                                profile.SecondDayProgress = 0;
                            else if (i == 2)
                                profile.ThirdDayProgress = 0;
                            else if (i == 3)
                                profile.FourthDayProgress = 0;
                            else if (i == 4)
                                profile.FifthDayProgress = 0;
                            else if (i == 5)
                                profile.SixthDayProgress = 0;
                        }
                        ++i;
                    }
                } catch { } // you must handle possible exception
                try
                {
                    profile.TodayStepCount += (int)todaysProgressJsonDeserialized.bucket[0].dataset[0].point[0].value[0].intVal;
                } catch { }
                profile.AverageStepCountForToday = (profile.FirstDayProgress
                                                    + profile.SecondDayProgress
                                                    + profile.ThirdDayProgress
                                                    + profile.FourthDayProgress
                                                    + profile.FifthDayProgress
                                                    + profile.SixthDayProgress
                                                    + profile.SeventhDayProgress) / 7;

                profile.LastFetchTime = DateTime.Now;
                _context.Update(profile);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        private async Task<string> GoogleFitApiPostAggregate(string token, string json)
        {
            return await _apiFetcher
                .FetchGoogleFitAPIAsync(
                "https://www.googleapis.com/fitness/v1/users/me/dataset:aggregate",
                token,
                "POST",
                json.ToString());
        }

        public async Task<IActionResult> Edit()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
                return RedirectToAction("Index");

            var profile = await _context.Profile.SingleOrDefaultAsync(m => m.Id == currentUser.ProfileId);

            if (profile == null)
            {
                return NotFound();
            }
            return View(profile);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Profile profile, IFormFile ProfilePictureFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser currentUser = await _userManager.GetUserAsync(User);

                    if (ProfilePictureFile != null)
                    {
                        string uploadPath = Path.Combine(_environment.WebRootPath, "uploads");
                        Directory.CreateDirectory(Path.Combine(uploadPath, currentUser.Id));
                        string filename = Path.GetFileName(ProfilePictureFile.FileName);

                        using (var fs = new FileStream(Path.Combine(uploadPath, currentUser.Id, filename), FileMode.Create))
                        {
                            await ProfilePictureFile.CopyToAsync(fs);
                        }
                        profile.ProfilePicture = filename;
                    }

                    profile.AverageStepCountForToday = (profile.FirstDayProgress
                                    + profile.SecondDayProgress
                                    + profile.ThirdDayProgress
                                    + profile.FourthDayProgress
                                    + profile.FifthDayProgress
                                    + profile.SixthDayProgress
                                    + profile.SeventhDayProgress) / 7;

                    _context.Update(profile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProfileExists(profile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }

        private bool ProfileExists(int id)
        {
            return _context.Profile.Any(e => e.Id == id);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}
