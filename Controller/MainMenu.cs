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
                    break;
                case "3":
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }

    public void AdminMenu()
    {
        
    }

    public void UserMenu()
    {
        
    }
}