using _7semester_ASP_FirstTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using System.Net.Mime;

namespace _7semester_ASP_FirstTask.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        ApplicationContext db;

        static string jsonUserData = @"{"""":""""}";

        private static ValueTuple<string, string> userDatum = JsonConvert.DeserializeObject<ValueTuple<string, string>>(jsonUserData);

        public HomeController(ILogger<HomeController> _logger, ApplicationContext db)
        {
            logger = _logger;
            this.db = db;
        }

        public async Task<IActionResult> Auth()
        {
            if (userDatum.Item1 == null && userDatum.Item2 == null)
                return View();
            else return RedirectToAction("Account", "Home");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Logout()
        {
            userDatum = (null, null);
            jsonUserData = JsonConvert.SerializeObject(userDatum);
            return RedirectToAction("Account", "Home");
        }

        public async Task<IActionResult> Account()
        {

            if (userDatum.Item1 != null && userDatum.Item2 != null)
            {
                foreach (var user in await db.Users.ToListAsync())
                {
                    if (user.login == userDatum.Item1 && user.password == userDatum.Item2)
                    {
                        ViewBag.Login = userDatum.Item1;
                        ViewData["password"] = userDatum.Item2;
                        return View();
                    }
                }
            }
            else return RedirectToAction("Auth", "Home");
            return RedirectToAction("Auth", "Home");
        }

        public async Task<IActionResult> ChangePassword(string OldPassword, string NewPassword)
        {
            if (userDatum.Item1 != null && userDatum.Item2 != null)
                if (OldPassword == userDatum.Item2)
                {
                    userDatum = (userDatum.Item1, NewPassword);
                    jsonUserData = JsonConvert.SerializeObject(userDatum);
                    foreach (var user in await db.Users.ToListAsync())
                    {
                        if (user.login == userDatum.Item1 && user.password == OldPassword)
                        {
                            user.password = NewPassword;
                            db.Update(user);
                            await db.SaveChangesAsync();

                        }
                    }
                }
            return RedirectToAction("Account", "Home");
            //return PartialView();
        }

        public async Task<IActionResult> CheckLogin(string login, string password)
        {
            var users = new List<User>(await db.Users.ToListAsync());
            foreach (var user in users)
            {
                if (user.login == login && user.password == password)
                {
                    userDatum = (login, password);
                    jsonUserData = JsonConvert.SerializeObject((login, password));
                    return RedirectToAction("Account", "Home");
                }
            }
            return RedirectToAction("Account", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}