using LoginFormsASPCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace LoginFormsASPCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly CodeFirstDbContext context;

        public HomeController(CodeFirstDbContext context)
        {
            this.context = context;
        }
         
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult lOGIN()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult lOGIN(UserTbl user)
        {
             
            var myUser =  context.UserTbls.Where(x => x.Email == user.Email && x.Password == user.Password).FirstOrDefault();
            if (myUser != null)
            {
                HttpContext.Session.SetString("UserSession",myUser.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                ViewBag.Message = "Login Failed";
            }
            return View(myUser);
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("LOGIN");
            }
            return View();
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                 HttpContext.Session.Remove("UserSession");
                return RedirectToAction("LOGIN");
            }
            return View();
        }

        public IActionResult Register()
        {
             
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserTbl user)
        {
            if(ModelState.IsValid)
            {
                await context.UserTbls.AddAsync(user);
                await context.SaveChangesAsync();
                TempData["Success"] = "Registered Successfully";
                return RedirectToAction("LOGIN");
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
