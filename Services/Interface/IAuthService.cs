using MINIBANK.Models;

namespace MINIBANK.Services.Interface
{
    public interface IAuthService
    {
        Users Login(string email, string password);
        void Register(Users user);
    }
}
