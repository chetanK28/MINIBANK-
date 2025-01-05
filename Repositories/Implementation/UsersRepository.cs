using MINIBANK.DBContext;
using MINIBANK.Models;
using MINIBANK.Repositories.Interface;
using System.Linq;

namespace MINIBANK.Repositories.Implementation
{
    public class UsersRepository : IUsers
    {
        private readonly Context cont;

        // Constructor with dependency injection
        public UsersRepository(Context cont)
        {
            this.cont = cont;
        }

        // Adds a user to the database
        public void AddUser(Users user)
        {
            cont.Users.Add(user);
            cont.SaveChanges();
        }

        // Deletes a user by ID
        public void DeleteUser(int id)
        {
            var user = cont.Users.FirstOrDefault(u => u.Id == id); // Find by Id
            if (user != null)
            {
                cont.Users.Remove(user);
                cont.SaveChanges();
            }
        }

        // Retrieves a user by ID
        public Users GetUserById(int id)
        {
            return cont.Users.FirstOrDefault(u => u.Id == id); // Find by Id
        }

        // Retrieves all users
        public IEnumerable<Users> GetAllUsers()
        {
            return cont.Users;
        }



        // Updates an existing user
        public void UpdateUser(Users user)
        {
            cont.Users.Update(user);
            cont.SaveChanges();
        }
    }
}
