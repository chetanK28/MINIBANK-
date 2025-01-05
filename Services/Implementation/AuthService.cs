using MINIBANK.Models;
using MINIBANK.Repositories.Implementation;
using MINIBANK.Repositories.Interface;
using MINIBANK.Services.Interface;

namespace MINIBANK.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IAuth auth;
        public AuthService(IAuth auth)
        {
            this.auth = auth;
        }

        public Users Login(string email, string password)
        {
            var user = auth.GetUserByEmail(email);
            if (user == null || user.Password != password)
                throw new UnauthorizedAccessException("Invalid credentials.");
            return user;
        }

        public void Register(Users user)
        {
            if (auth.GetUserByEmail(user.Email) != null)
                throw new UnauthorizedAccessException("User Already exists.");
            auth.AddUser(user);
        }
    }
}
