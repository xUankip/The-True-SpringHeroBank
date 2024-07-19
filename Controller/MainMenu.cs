﻿using The_True_SpringHeroBank.Entity;

namespace The_True_SpringHeroBank.Controller;

public class MainMenu
{ 
    
    public void Main()
    {
        UserController userController = new UserController();
        while (true)
        {
            Console.WriteLine("------------Spring Hero Bank------------");
            Console.WriteLine("1. Register.");
            Console.WriteLine("2. Login.");
            Console.WriteLine("3. Exit.");
            Console.WriteLine("——————————————————-");
            Console.Write("Type your choice (1,2,3): ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    userController.Register();
                    break;
                case "2":
                    userController.Login();
                    break;
                case "3":
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }

    public void AdminMenu(User user)
    { 
        UserController userController = new UserController();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("------ Spring Hero Bank ------");
            Console.WriteLine($"Welcome back Admin {user.FullName}");
            Console.WriteLine("1. User List.");
            Console.WriteLine("2. Transaction List");
            Console.WriteLine("3. Find User By Name");
            Console.WriteLine("4. Find User By Account Number");
            Console.WriteLine("5. Find User by Phone Number");
            Console.WriteLine("6. Add New User.");
            Console.WriteLine("7. Lock and Unlock Account");
            Console.WriteLine("8. Find Transaction History by Account Number ");
            Console.WriteLine("9. Change Account Information");
            Console.WriteLine("10. Change Account Password");
            Console.WriteLine("11. Exit.");
            Console.WriteLine("----------------------------------------");
            Console.Write("Type your choice (1 to 11): ");
            var choice = Console.ReadLine();
            Console.WriteLine(choice);
            switch (choice)
            {
                case "1":
                    userController.DisplayUsers();
                    break;
                case "2":
                    // TransactionList
                    break;
                case "3":
                    userController.SearchUsersByName();
                    break;
                case "4":
                    userController.SearchUsersByAccountNumber();
                    break;
                case "5":
                    userController.SearchUsersByPhone();
                    break;
                case "6":
                    // Register();
                    break;
                case "7":
                    // LockUnlockUserAccount();
                    break;
                case "8":
                    // SearchTransactionHistoryByAccountNumber();
                    break;
                case "9":
                    // UpdateAccountInfo();
                    break;
                case "10":
                    // UpdateAccountPassword();
                    break;
                case "11":
                    break;
                default:
                    Console.WriteLine("Invalid choice, Try Again!");
                    break;
            }
            Console.WriteLine("Enter to continue!");
            Console.ReadLine();
        }
    }

    public void UserMenu(User user)
    {
        
    }
}