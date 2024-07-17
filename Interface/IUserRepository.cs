using The_True_SpringHeroBank.Entity;

namespace The_True_SpringHeroBank.Interface;

public interface IUserRepository
{
    public void AddUser(User user);
    User FindByAccountNumber(string accountNumber);
    List<User> FindByFullName(string fullName);
    List<User> FindByPhoneNumber(string phoneNumber);
    List<User> FindAll();
    public User Login(string userName, string passWord);
    public void Register();
    public void Deposit();
    public void Withdraw();
    public void Transfer();
    List<Transaction> Transactions();
}