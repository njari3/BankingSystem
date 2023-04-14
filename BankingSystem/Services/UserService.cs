using BankingSystem.Data;
using BankingSystem.Models;

namespace BankingSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUser(int id) => await _userRepository.GetById(id);

    }
}
