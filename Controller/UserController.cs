using The_True_SpringHeroBank.Entity;
using The_True_SpringHeroBank.Repository;

namespace The_True_SpringHeroBank.Controller;

public class UserController
{
    private UserRepository _userRepository = new UserRepository();
    private User _user = new User();
    private MainMenu _menu = new MainMenu();
    public void Register()
    {       
        var user = new User();
        Console.WriteLine("Type Information Below:");
        Console.WriteLine("Input User Name: ");
        user.UserName = Console.ReadLine();
        Console.WriteLine("Input Full Name: ");
        user.FullName = Console.ReadLine();
        Console.WriteLine("Input Phone Number: ");
        user.PhoneNumber = Console.ReadLine();
        Console.WriteLine("Input PassWord: ");
        user.PassWord = Console.ReadLine();
        Console.WriteLine("Type of User 1 for 'Admin' or 2 for 'Regular User'");
        var type = Console.ReadLine();
        if (type == "1")
        {
            user.Type = User.UserType.Admin;
        }
        else if (type == "2")
        {
            user.Type = User.UserType.RegularUser;
        }else
        {
            Console.WriteLine("Invalid Choice");
        }

        Random random = new Random();
        string randomDigits = "";
        for (int i = 0; i < 10; i++)
        {
            randomDigits += random.Next(0, 10).ToString();
        }
        user.AccountNumber = randomDigits;
        _userRepository.AddUser(user);
    }
    public void Deposit()
    {
        
    }

    public void Login()
    {
        Console.WriteLine("Username :");
        var username = Console.ReadLine();
        Console.WriteLine("Password: ");
        var password = Console.ReadLine();
        var user = _userRepository.FindAll()
            .FirstOrDefault(_user => _user.UserName == username && _user.PassWord == password);
        if (user != null)
        {
            if (user.Type == User.UserType.Admin)
            {
                _menu.AdminMenu();
            }
            else
            {
                _menu.UserMenu();
            }
        }
    }
}