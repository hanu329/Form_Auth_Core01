using FormAuthCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FormAuthCore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly FormCoreAuthContext _db;
        public HomeController(FormCoreAuthContext db)
        {
            _db = db;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var res = _db.Users.ToList();
            return View(res);
        }
        public IActionResult Create()
        { 
            return View();
        }
        [HttpPost]
        public IActionResult Create(UserModel model)
        {
            _db.Users.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles ="admin")]
        public IActionResult Edit(int id)
        {
            var usr = _db.Users.Find(id);

            return View(usr);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(UserModel model)
        {
            _db.Users.Update(model);
            _db.SaveChanges();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
