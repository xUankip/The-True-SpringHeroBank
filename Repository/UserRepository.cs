using System.Data;
using System.Data.Common;
using MySqlConnector;
using The_True_SpringHeroBank.Entity;
using The_True_SpringHeroBank.Interface;

namespace The_True_SpringHeroBank.Repository;

public class UserRepository
{
    private const string MySqlConnectionString = "server=127.0.0.1;uid=root;" + "pwd=;database=the_true_spring_hero_bank";
    public void AddUser(User user)
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
        command.Parameters.AddWithValue("@AccountNumber",user.AccountNumber);
        command.Parameters.AddWithValue("@Balance", 0);
        command.Parameters.AddWithValue("@Type", user.Type.ToString());
        command.Parameters.AddWithValue("@Status",1);
        command.ExecuteNonQuery();
        conn.Close();
        Console.WriteLine("Sign Up Successfully");
    }

    public User findByInfo(string info, string value)
    {
        User user = null;
        try
        {
            using (var conn = new MySqlConnection(MySqlConnectionString))
            {
                conn.Open();
                string query =
                    $"SELECT Id, AccountNumber, UserName, FullName, PhoneNumber, Balance, Status FROM users WHERE {info} = @value";
                var command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@value", value);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32("Id"),
                            UserName = reader.GetString("UserName"),
                            FullName = reader.GetString("FullName"),
                            AccountNumber = reader.GetString("AccountNumber"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            Balance = reader.GetDouble("Balance"),
                            Status = reader.GetInt32("Status")
                        };
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return user;
    }

    public User FindByFullName(string fullName)
    {
        return findByInfo("FullName", fullName);
    }

    public User FindByPhoneNumber(string phoneNumber)
    {
        return findByInfo("PhoneNumber", phoneNumber);
    }

    public User FindByAccountNumber(string accountNumber)
    {
        return findByInfo("AccountNumber", accountNumber);
    }

    public List<User> FindAll()
    {
        List<User> users = new List<User>();
        var conn = new MySqlConnection(MySqlConnectionString);
        try
        {
            conn.Open();
            string query = "SELECT * FROM users";
            var command = new MySqlCommand(query, conn);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User()
                {
                    Id = reader.GetInt32("Id"),
                    AccountNumber = reader["AccountNumber"] != DBNull.Value ? reader.GetString("AccountNumber") : string.Empty,
                    UserName = reader["UserName"] != DBNull.Value ? reader.GetString("UserName") : string.Empty,
                    PassWord = reader["Password"] != DBNull.Value ? reader.GetString("Password") : string.Empty,
                    FullName = reader["FullName"] != DBNull.Value ? reader.GetString("FullName") : string.Empty,
                    PhoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader.GetString("PhoneNumber") : string.Empty,
                    Balance = reader["Balance"] != DBNull.Value ? reader.GetDouble("Balance") : 0.0,
                    Type = (User.UserType)Enum.Parse(typeof(User.UserType), reader.GetString("Type")),
                    Status = reader["Status"] != DBNull.Value ? reader.GetInt32("Status") : 0
                });
            }

            reader.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
        finally
        {
            conn.Close();
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
                Id = reader.GetInt32("Id"),
                UserName = reader.GetString("Username"),
                PassWord = reader.GetString("Password"),
                FullName = reader.GetString("FullName"),
                PhoneNumber = reader.GetString("PhoneNumber"),
                AccountNumber = reader.GetString("AccountNumber"),
                Balance = reader.GetDouble("Balance"),
                Type = (User.UserType)Enum.Parse(typeof(User.UserType), reader.GetString("Type")),
                Status = reader.GetInt32("Status")
            };
            Console.WriteLine("Login Success");
            return user;
        }
        Console.WriteLine("Wrong UserName Or PassWord");
        conn.Close();
        return null;
    }

    public User CheckBalance(string accountNumber)
    {
        User user = new User();
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            string query = "SELECT Balance FROM users WHERE AccountNumber = @accountNumber";
            var command = new MySqlCommand(query, conn);
            command.Parameters.AddWithValue("@accountNumber", accountNumber);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                user.Balance = reader.GetDouble("Balance");
            }
            conn.Close();
        }
        return user;
    }

    public User EditInfomation(User user, string accountNumber)
    {
        try
        {
            using (var conn = new MySqlConnection(MySqlConnectionString))
            {
                conn.Open();
                string query = "UPDATE users set FullName = @fullName, phoneNumber = @phoneNumber WHERE AccountNumber = @accountNumber";
                var command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@accountNumber", accountNumber);
                command.Parameters.AddWithValue("@fullName", user.FullName);
                command.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
                command.ExecuteNonQuery();
                Console.WriteLine("Edit Successful!");
                conn.Close();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return user;
    }

    public bool EditPassword(User user, string currentPassword, string newPassword)
    {
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                // Check current Password
                string query = "SELECT PassWord FROM users WHERE Id = @Id";
                var command = new MySqlCommand(query, conn, transaction);
                command.Parameters.AddWithValue("@Id", user.Id);
                string storedPassword = command.ExecuteScalar()?.ToString();

                if (storedPassword != currentPassword)
                {
                    Console.WriteLine("Current password is incorrect.");
                    return false;
                }

                // Update New Password
                string updatePasswordQuery = "UPDATE users SET PassWord = @newPassword WHERE Id = @Id";
                var updateCommand = new MySqlCommand(updatePasswordQuery, conn, transaction);
                updateCommand.Parameters.AddWithValue("@newPassword", newPassword);
                updateCommand.Parameters.AddWithValue("@Id", user.Id);
                updateCommand.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine(e);
                return false;
            }
        }
    }

    public bool ChangeStatus(string accountNumber)
    {
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                // current status
                string getStatusQuery = "SELECT Status FROM users WHERE AccountNumber = @accountNumber";
                var getStatusCommand = new MySqlCommand(getStatusQuery, conn, transaction);
                getStatusCommand.Parameters.AddWithValue("@accountNumber", accountNumber);
                var reader = getStatusCommand.ExecuteReader();

                if (reader.Read())
                {
                    int currentStatus = reader.GetInt32("Status");
                    int newStatus = currentStatus == 1 ? 0 : 1;
                    reader.Close(); // Đóng reader trước khi thực hiện truy vấn khác

                    // Update status
                    string updateStatusQuery =
                        "UPDATE users SET Status = @newStatus WHERE AccountNumber = @accountNumber";
                    var updateStatusCommand = new MySqlCommand(updateStatusQuery, conn, transaction);
                    updateStatusCommand.Parameters.AddWithValue("@newStatus", newStatus);
                    updateStatusCommand.Parameters.AddWithValue("@accountNumber", accountNumber);
                    updateStatusCommand.ExecuteNonQuery();

                    transaction.Commit();
                    return true;
                }
                else
                {
                    Console.WriteLine("Account not found.");
                    return false;
                }
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine("Error: " + e.Message);
            }

            return false;
        }
    }
    
}