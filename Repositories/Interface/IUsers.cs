using MINIBANK.Models;

namespace MINIBANK.Repositories.Interface
{
    public interface IUsers
    {
        public void AddUser(Users user);
        public void UpdateUser(Users user);
        public void DeleteUser(int id);
        public Users GetUserById(int id);
        public IEnumerable<Users> GetAllUsers();
    }
}
