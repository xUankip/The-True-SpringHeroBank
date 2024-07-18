using System.Data;
using MySqlConnector;
using The_True_SpringHeroBank.Entity;
using The_True_SpringHeroBank.Interface;

namespace The_True_SpringHeroBank.Repository;

public class UserRepository : IUserRepository
{
    private List<User> users = new List<User>();
    private const string MySqlConnectionString = "server=127.0.0.1;uid=root;" + "pwd=;database=the_true_spring_hero_bank";
    public User AddUser(User user)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        string query = "INSERT INTO users (Username, Password, FullName, PhoneNumber, AccountNumber, Balance, Type, Status) " +
                       "VALUES (@Username, @Password, @FullName, @PhoneNumber, @AccountNumber, @Balance, @Type, @Status)";
        var command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Username", user.UserName);
        command.Parameters.AddWithValue("@Password", user.PassWord);
        command.Parameters.AddWithValue("@FullName", user.FullName);
        command.Parameters.AddWithValue("@PhoneNumber", user.PhoneNumber);
        command.Parameters.AddWithValue("@AccountNumber", user.AccountNumber);
        command.Parameters.AddWithValue("@Balance", 0);
        command.Parameters.AddWithValue("@Type", user.Type.ToString());
        command.Parameters.AddWithValue("@Status",1);
        command.ExecuteNonQuery();
        conn.Close();
        Console.WriteLine("Sign Up Successfully");
        return user;
    }

    public User FindByAccountNumber(string accountNumber)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        string query = "SELECT * FROM users WHERE AccountNumber = @AccountNumber";
        var command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@AccountNumber", accountNumber);
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new User()
            {
                Id = reader.GetInt32("Id"),
                UserName = reader.GetString("UserName"),
                PassWord = reader.GetString("Password"),
                FullName = reader.GetString("FullName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                Balance = reader.GetDouble("Balance"),
                Type = (User.UserType)Enum.Parse(typeof(User.UserType), reader.GetString("Type")),
                Status = reader.GetInt32("Status")
            };
        }
        return null;
    }

    public List<User> FindByFullName(string fullName)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        string query = "SELECT * FROM users WHERE FullName LIKE @FullName";
        var command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@FullName", "---" + fullName + "---");
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User()
            {
                Id = reader.GetInt32("Id"),
                UserName = reader.GetString("UserName"),
                PassWord = reader.GetString("Password"),
                FullName = reader.GetString("FullName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                Balance = reader.GetDouble("Balance"),
                Type = (User.UserType)Enum.Parse(typeof(User.UserType), reader.GetString("Type")),
                Status = reader.GetInt32("Status")
            });
        }
        return users;
    }

    public List<User> FindByPhoneNumber(string phoneNumber)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        string query = "SELECT * FROM users WHERE PhoneNumber LIKE @PhoneNumber";
        var command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@FullName", "---" + phoneNumber + "---");
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User()
            {
                Id = reader.GetInt32("Id"),
                UserName = reader.GetString("UserName"),
                PassWord = reader.GetString("Password"),
                FullName = reader.GetString("FullName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                Balance = reader.GetDouble("Balance"),
                Type = (User.UserType)Enum.Parse(typeof(User.UserType), reader.GetString("Type")),
                Status = reader.GetInt32("Status")
            });
        }
        return users;
    }

    public List<User> FindAll()
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        string query = "SELECT * FROM users";
        var command = new MySqlCommand(query, conn);
        var reader = command.ExecuteReader();
        while (reader.Read())
        {
            users.Add(new User()
            {
                Id = reader.GetInt32("Id"),
                UserName = reader.GetString("UserName"),
                PassWord = reader.GetString("Password"),
                FullName = reader.GetString("FullName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                Balance = reader.GetDouble("Balance"),
                Type = (User.UserType)Enum.Parse(typeof(User.UserType), reader.GetString("Type")),
                Status = reader.GetInt32("Status")
            });
        }
        return users;
    }

    public User Login(string userName, string passWord)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        string query = "SELECT * FROM users WHERE Username = @Username AND Password = @Password";
        var command = new MySqlCommand(query, conn);
        command.Parameters.AddWithValue("@Username", userName);
        command.Parameters.AddWithValue("@Password", passWord);
        var reader = command.ExecuteReader();
        if (reader.Read())
        {
            if (reader.GetInt32(0) == 0)
            {
                Console.WriteLine("Account LOCKED");
            }
            User user = new User
            {
                Id = reader.GetInt32("UserId"),
                UserName = reader.GetString("Username"),
                PassWord = reader.GetString("Password"),
                FullName = reader.GetString("FullName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                AccountNumber = reader.GetString("AccountNumber"),
                Balance = reader.GetDouble("Balance"),
                Status = reader.GetInt32("Status")
            };
            Console.WriteLine("Login Success");
            return user;
        }
        Console.WriteLine("Wrong UserName Or PassWord");
        return null;
    }

    public void Deposit()
    {
        throw new NotImplementedException();
    }

    public void Withdraw()
    {
        throw new NotImplementedException();
    }

    public void Transfer()
    {
        throw new NotImplementedException();
    }

    public List<Transaction> Transactions()
    {
        throw new NotImplementedException();
    }
}