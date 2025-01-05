using MINIBANK.Models;

namespace MINIBANK.Repositories.Interface
{
    public interface IAuth
    {
        Users GetUserByEmail(string email);
        void AddUser(Users user);
    }
}
