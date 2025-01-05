using MINIBANK.Models;

namespace MINIBANK.Services.Interface
{
    public interface IUserService
    {
        public void AddUserService(Users user);
        public void UpdateUserService(Users user);
        public void DeleteUserService(int id);
        public Users GetUserByIdService(int id);
        public IEnumerable<Users> GetAllUsersService();

        public IEnumerable<Users> GetAllUsers();
    }
}
