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

    public void DisplayUsers()
    {
        List<User> users = _userRepository.FindAll();
        Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20} | {6, -20} |{7, -20} ",
            "Id", "Account NUmber", "User Name", "", "Full Name", "Phone Number", "Balance", "Status");
        foreach (var user in users)
        {
            Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20} | {6, -20}  ",
                user.Id, user.AccountNumber, user.UserName, user.FullName, user.PhoneNumber, user.Balance, user.Status);
        }
    }

    public void SearchUsersByName()
    {
        Console.WriteLine("Type Full Name");
        string fullName = Console.ReadLine();
        _userRepository.FindByFullName(fullName);
    }

    public void Login()
    {
        Console.WriteLine("Username :");
        var username = Console.ReadLine();
        Console.WriteLine("Password: ");
        var password = Console.ReadLine();
        var user = _userRepository.Login(username, password);
        if (user != null)
        {
            if (user.Type == User.UserType.Admin)
            {
                _menu.AdminMenu(user);
            }
            else
            {
                _menu.UserMenu(user);
            }
        }
    }
}