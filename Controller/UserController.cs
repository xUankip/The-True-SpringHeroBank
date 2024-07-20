using System.Threading.Channels;
using The_True_SpringHeroBank.Entity;
using The_True_SpringHeroBank.Repository;

namespace The_True_SpringHeroBank.Controller;

public class UserController
{
    private UserRepository _userRepository = new UserRepository();
    private TransactionRepository _transactionRepository = new TransactionRepository();
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

    public void DisplayUsers()
    {
        List<User> users = _userRepository.FindAll();
        Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20} | {6, -20} | {7, -20} ",
            "Id", "Account Number", "User Name", "Full Name", "Phone Number", "Balance", "Type", "Status");

        foreach (var user in users)
        {
            Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20} | {6, -20} | {7, -20} ",
                user.Id, user.AccountNumber, user.UserName, user.FullName, user.PhoneNumber, user.Balance, user.Type, user.Status);
        }
    }

    public void DisplayByInfo(User user)
    {
        Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20} | {6, -20} | {7, -20} ",
            "Id", "Account Number", "User Name", "Full Name", "Phone Number", "Balance", "Type", "Status");   
        Console.WriteLine("{0, -10} | {1, -20} | {2, -20} | {3, -20} | {4, -20} | {5, -20} | {6, -20} | {7, -20} ",
            user.Id, user.AccountNumber, user.UserName, user.FullName, user.PhoneNumber, user.Balance, user.Type, user.Status);
    }
    public void SearchUsersByName()
    {
        Console.WriteLine("Type Full Name");
        string fullName = Console.ReadLine();
        User user = _userRepository.FindByFullName(fullName);
        DisplayByInfo(user);
    }

    public void SearchUsersByAccountNumber()
    {
        Console.WriteLine("Type Account Number");
        string accountNumber = Console.ReadLine();
        User user = _userRepository.FindByAccountNumber(accountNumber);
        DisplayByInfo(user);
    }

    public void SearchUsersByPhone()
    {
        Console.WriteLine("Type Phone Number");
        string phoneNumber = Console.ReadLine();
        User user = _userRepository.FindByPhoneNumber(phoneNumber);
        DisplayByInfo(user);
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

    
    public void Deposit()
    {
        TransactionRepository transactionRepository = new TransactionRepository();
        User user = new User();
        Console.WriteLine("Enter your Password");
        string pass = Console.ReadLine();
        if (pass == user.PassWord)
        {
            Console.WriteLine("Enter the amount you want to deposit");
            double amount = Convert.ToDouble(Console.ReadLine());
            bool success = transactionRepository.UserDeposit(user, amount);
            if (amount > 0 && success)
            {
                Console.WriteLine("Deposit Successful!");
                DisplayByInfo(user);
            }
        }
    }

    public void Withdraw()
    {
        User user = new User();
        Console.WriteLine("Enter your Password");
        string pass = Console.ReadLine();
        double balance = user.Balance;
        if (pass == user.PassWord)
        {
            Console.WriteLine("Enter the amount you want to deposit");
            double amount = Convert.ToDouble(Console.ReadLine());
            bool success = _transactionRepository.UserWithdraw(user, amount);
            if (amount > 0 && amount < balance && success)
            {
                Console.WriteLine("Withdraw Successful!");
                DisplayByInfo(user);
            }
        }
    }

    public void Transfer()
    {
        User user = new User();
        Console.WriteLine("Enter Sender Account Number");
        string sender = Console.ReadLine();
        _userRepository.FindByAccountNumber(sender);
        if (sender != null)
        {
            Console.WriteLine("Enter Receiver Account Number");
            string receiver = Console.ReadLine();
            if (receiver == user.AccountNumber)
            {
                Console.WriteLine("Enter Amount to Transfer");
                double amount = Convert.ToDouble(Console.ReadLine());
                bool success = _transactionRepository.UserTransfer(sender, receiver, amount);
            }
        }
       
    }
}