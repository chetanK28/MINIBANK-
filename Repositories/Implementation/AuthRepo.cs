using MINIBANK.DBContext;
using MINIBANK.Models;
using MINIBANK.Repositories.Interface;

namespace MINIBANK.Repositories.Implementation
{
    public class AuthRepo : IAuth
    {
        private Context cont;
        public AuthRepo(Context cont)
        {
            this.cont = cont;
        }
        public void AddUser(Users user)
        {
            cont.Users.Add(user);
            cont.SaveChanges();
        }

        public Users GetUserByEmail(string email)
        {
            return cont.Users.FirstOrDefault(u => u.Email == email);
        }
    }
}
