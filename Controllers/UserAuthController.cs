using FormAuthCore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FormAuthCore.Controllers
{
    public class UserAuthController : Controller
    {
        private readonly FormCoreAuthContext _db;
        public UserAuthController(FormCoreAuthContext db)
        {
            _db = db;
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(UserModel model)
        {
            _db.Users.Add(model);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var usr = _db.AuthUsers.FirstOrDefault(x=>x.Username==model.Username);
                //  fetch role here with id

                var userRoles = _db.UserRoles.Where(u => u.UserId == usr.Id).ToList();
                var roleIds = userRoles.Select(s => s.RoleId).ToList(); //[2,3]
                var roles = _db.Roles.Where(r => roleIds.Contains(r.Id)).ToList();
         
                if (usr.Password != model.Password)               
                {
                    throw new Exception("invaid credentials!");
                }
               
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                
            };
                foreach(var role in roles)
                {
                    var newRole = new Claim(ClaimTypes.Role, role.Name);
                    claims.Add(newRole);
                }

              
                var identity = new ClaimsIdentity(claims, "MyCookieAuthenticationScheme");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuthenticationScheme", principal);

            } 
            //role here
            return RedirectToAction("Index","Home");        
        }
        public IActionResult Signout()
        {
            return View("Login");
        }
    }
}
