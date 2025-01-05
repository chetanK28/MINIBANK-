using MINIBANK.Models;
using MINIBANK.Repositories.Interface;
using MINIBANK.Services.Interface;

namespace MINIBANK.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUsers usersRepository;

        // Constructor with correct dependency injection
        public UserService(IUsers usersRepository)
        {
            this.usersRepository = usersRepository; // Correctly assign the parameter to the field
        }

        public void AddUserService(Users user)
        {
            usersRepository.AddUser(user);
        }

        public void DeleteUserService(int id)
        {
            usersRepository.DeleteUser(id);
        }

        public IEnumerable<Users> GetAllUsers()
        {
            return usersRepository.GetAllUsers();
        }

        public IEnumerable<Users> GetAllUsersService()
        {
            Console.WriteLine(usersRepository.GetAllUsers().ToString);
            return usersRepository.GetAllUsers();
        }

        public Users GetUserByIdService(int id)
        {
            return usersRepository.GetUserById(id);
        }

        public void UpdateUserService(Users user)
        {
            usersRepository.UpdateUser(user);
        }
    }
}
