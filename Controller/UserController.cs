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

    public void DisplayBalance(User user)
    {
        Console.WriteLine("AccountNumber" +user.AccountNumber);
        Console.WriteLine("Balance" +user.Balance);
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
        Console.WriteLine("Enter your Account Number:");
        string accountNumber = Console.ReadLine();
        User user = _userRepository.FindByAccountNumber(accountNumber);

        if (user != null)
        {
            Console.WriteLine("Enter the amount you want to deposit:");
            double amount = Convert.ToDouble(Console.ReadLine());

            if (amount > 0)
            {
                bool success = _transactionRepository.UserDeposit(user, amount);
                if (success)
                {
                    Console.WriteLine("Deposit Successful!");
                    DisplayBalance(user);
                }
                else
                {
                    Console.WriteLine("Deposit Failed.");
                }
            }
            else
            {
                Console.WriteLine("Amount must be greater than zero.");
            }
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    public void Withdraw()
    {
        Console.WriteLine("Enter your Account Number:");
        string accountNumber = Console.ReadLine();
        User user = _userRepository.FindByAccountNumber(accountNumber);

        if (user != null)
        {
            Console.WriteLine("Enter the amount you want to With draw:");
            double amount = Convert.ToDouble(Console.ReadLine());

            if (amount > 0 && amount < user.Balance)
            {
                bool success = _transactionRepository.UserWithdraw(user, amount);
                if (success)
                {
                    Console.WriteLine("With draw Successful!");
                    DisplayBalance(user);
                }
                else
                {
                    Console.WriteLine("With draw Failed.");
                }
            }
            else
            {
                Console.WriteLine("Amount must be greater than zero.");
            }
        }
        else
        {
            Console.WriteLine("User not found.");
        }
    }

    public void Transfer()
    {
        Console.WriteLine("Enter Sender Account Number");
        string senderAccountNumber = Console.ReadLine();
        User sender = _userRepository.FindByAccountNumber(senderAccountNumber);

        if (sender != null)
        {
            Console.WriteLine("Enter Receiver Account Number");
            string receiverAccountNumber = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(receiverAccountNumber))
            {
                Console.WriteLine("Enter Amount to Transfer");
                if (double.TryParse(Console.ReadLine(), out double amount))
                {
                    if (amount > 0)
                    {
                        bool success = _transactionRepository.UserTransfer(sender, receiverAccountNumber, amount);

                        if (success)
                        {
                            Console.WriteLine("Transfer successful!");
                        }
                        else
                        {
                            Console.WriteLine("Transfer failed.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Amount must be greater than zero.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid amount entered.");
                }
            }
            else
            {
                Console.WriteLine("Invalid receiver account number.");
            }
        }
        else
        {
            Console.WriteLine("Sender account not found.");
        }
    }

    public void QueryBalance()
    {
            Console.WriteLine("Enter your Account Number");
            var accountNumber = Console.ReadLine();
            User user = _userRepository.CheckBalance(accountNumber);
            Console.WriteLine("Your Balance : " + user.Balance);
    }

    public void UpdatePersonalInfo()
    {
        Console.WriteLine("Enter Account number");
        var accountNumber = Console.ReadLine();
        User user = _userRepository.FindByAccountNumber(accountNumber);
        if (accountNumber == user.AccountNumber)
        {
            Console.WriteLine("Enter New Full Name");
            user.FullName = Console.ReadLine();
            Console.WriteLine("Enter New Phone Number");
            user.PhoneNumber = Console.ReadLine();
            _userRepository.EditInfomation(user, accountNumber);
        }
        else
        {
            Console.WriteLine("Account not Found");
        }
    }

    public void UpdatePassword()
    {
        Console.WriteLine("Enter your Account Number:");
        string accountNumber = Console.ReadLine();
        var user = _userRepository.FindByAccountNumber(accountNumber);
        if (user == null)
        {
            Console.WriteLine("Account not found.");
            return;
        }

        Console.WriteLine("Enter your current Password:");
        string currentPassword = Console.ReadLine();

        Console.WriteLine("Enter your new Password:");
        string newPassword = Console.ReadLine();

        Console.WriteLine("Re-enter your new Password:");
        string confirmPassword = Console.ReadLine();

        if (newPassword != confirmPassword)
        {
            Console.WriteLine("New passwords do not match.");
            return;
        }

        bool success = _userRepository.EditPassword(user, currentPassword, newPassword);
        if (success)
        {
            Console.WriteLine("Password changed successfully!");
        }
        else
        {
            Console.WriteLine("Failed to change password.");
        }
    }

    public void LockUnlockUserAccount()
    {
        Console.WriteLine("Enter Account Number:");
        string accountNumber = Console.ReadLine();

        bool success = _userRepository.ChangeStatus(accountNumber);

        if (success)
        {
            Console.WriteLine("Account status change successfully!");
        }
        else
        {
            Console.WriteLine("Failed to change status account status.");
        }
    }
}