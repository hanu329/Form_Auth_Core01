using FormAuthCore.Models;
using Microsoft.AspNetCore.Mvc;
using FormAuthCore.Interfaces;

namespace FormAuthCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _auth;
        public AuthController(IAuthServices auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        public string Login([FromBody] LoginRequest obj)
        {
            var token = _auth.Login(obj);

            // return token;
            return token;
        }

        [HttpPost("assignRole")]
        public bool AssignRoleToUser([FromBody] AddUserRole userRole)
        {
            var addedUserRole = _auth.AssignRoleToUser(userRole);
            return addedUserRole;
        }

        [HttpPost("addUser")]
        public User AddUser([FromBody] User user)
        {
            var addeduser = _auth.AddUser(user);
            return addeduser;
        }

        [HttpPost("addRole")]
        public Role AddRole([FromBody] Role role)
        {
            var addedRole = _auth.AddRole(role);
            return addedRole;
        }

    }
}