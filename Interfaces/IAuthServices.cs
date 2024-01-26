using FormAuthCore.Models;

namespace FormAuthCore.Interfaces
{
    public interface IAuthServices
    {
        User AddUser(User user);
        string Login(LoginRequest loginRequest);
        Role AddRole(Role role);
        bool AssignRoleToUser(AddUserRole obj);
    }
}
