using FormAuthCore.Models;
using FormAuthCore.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FormAuthCore.Interfaces;
using Microsoft.CodeAnalysis.Scripting;


namespace FormAuthCore.Services
{
    public class AuthService : IAuthServices
    {
        private readonly FormCoreAuthContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(FormCoreAuthContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Role AddRole(Role role)
        {
            var addedRole = _context.Roles.Add(role);
            _context.SaveChanges();
            return addedRole.Entity;
        }

        public User AddUser(User user)
        {
            //var pwd = ASCIIEncoding.ASCII.GetBytes(user.Password);
            //var encPwd = Convert.ToBase64String(pwd);

            // user.Password = encPwd;
           user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var addedUser = _context.AuthUsers.Add(user);
            _context.SaveChanges();
            return addedUser.Entity;
        }

        public bool AssignRoleToUser(AddUserRole obj)
        {
            try
            {
                var addRoles = new List<UserRole>();
                var user = _context.AuthUsers.SingleOrDefault(s => s.Id == obj.UserId);
                var ch = user;
                if (user == null)
                    throw new Exception("user is not valid");
                foreach (int role in obj.RoleIds)
                {
                    var userRole = new UserRole();
                    userRole.RoleId = role;
                    userRole.UserId = user.Id;
                    addRoles.Add(userRole);
                }
                _context.UserRoles.AddRange(addRoles);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public string Login(LoginRequest loginRequest)
        {
            // var encPwd = Convert.FromBase64String(loginRequest.Password);
            //var decryptPass = ASCIIEncoding.ASCII.GetString(loginRequest.Password);
      
            if (loginRequest.Username != null && loginRequest.Password != null)
            {
               // &&s.Password==loginRequest.Password
                var user = _context.AuthUsers.SingleOrDefault(s => s.Username == loginRequest.Username);
                var isvalid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
                //if(!isValid)
                //{
                //    throw new Exception("Invlid Password!");
                //}
                if (user != null)
                {
                    var claims = new List<Claim> {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.Name)
                    };
                    var userRoles = _context.UserRoles.Where(u => u.UserId == user.Id).ToList(); //[{1:2},{1,3}]
                    var roleIds = userRoles.Select(s => s.RoleId).ToList(); //[2,3]
                    var roles = _context.Roles.Where(r => roleIds.Contains(r.Id)).ToList(); //[{id=2:Name="cust"},{3:"emp"}]
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.AddMinutes(10),
                        signingCredentials: signIn);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwtToken;
                }
                else
                {
                    throw new Exception("user is not valid");
                }
            }
            else
            {
                throw new Exception("credentials are not valid");
            }
        }
    }
}