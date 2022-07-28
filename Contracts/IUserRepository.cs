using Shape.Models;

namespace Shape.Contracts
{
    public interface IUserRepository
    {
        Task<User> SignUpAsync(User user);
    }
}
