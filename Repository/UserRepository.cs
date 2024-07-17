using The_True_SpringHeroBank.Entity;
using The_True_SpringHeroBank.Interface;

namespace The_True_SpringHeroBank.Repository;

public class UserRepository : IUserRepository
{
    private List<User> users = new List<User>();
    public void AddUser(User user)
    {
        users.Add(user);
    }

    public User FindByAccountNumber(string accountNumber)
    {
        return users.FirstOrDefault(user => user.AccountNumber == accountNumber);
    }

    public List<User> FindByFullName(string fullName)
    {
        return users.Where(user => user.FullName.Contains(fullName, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<User> FindByPhoneNumber(string phoneNumber)
    {
        return users.Where(user => user.PhoneNumber.Contains(phoneNumber)).ToList();
    }

    public List<User> FindAll()
    {
        return users;
    }
}