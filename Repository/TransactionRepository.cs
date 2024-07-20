using MySqlConnector;
using The_True_SpringHeroBank.Entity;

namespace The_True_SpringHeroBank.Repository;

public class TransactionRepository
{
    private const string MySqlConnectionString = "server=127.0.0.1;uid=root;" + "pwd=;database=the_true_spring_hero_bank";
    public bool UserDeposit(User user, double amount)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        try
        {
            string updateBalanceQuery = "UPDATE users SET Balance = Balance + @amount WHERE Id = @Id";
            var command = new MySqlCommand(updateBalanceQuery, conn);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.ExecuteNonQuery();

            string insertTransactionQuery = "INSERT INTO transactions (CreatedAt, Type, Amount, SenderAccountNumber, ReciverAccountNumber, BalanceAfter, Id)" +
                                            " VALUES (@createdAt, @type, @amount, @senderAccountNumber, @reciverAccountNumber, @balanceAfter, @Id)";
            var command2 = new MySqlCommand(insertTransactionQuery, conn);
            command2.Parameters.AddWithValue("@createdAt", DateTime.Now);
            command2.Parameters.AddWithValue("@type", Transaction.TransactionType.Deposit.ToString());
            command2.Parameters.AddWithValue("@amount", amount);
            command2.Parameters.AddWithValue("@senderAccountNumber", user.AccountNumber);
            command2.Parameters.AddWithValue("@reciverAccountNumber", user.AccountNumber);
            command2.Parameters.AddWithValue("@balanceAfter", user.Balance + amount);
            command2.Parameters.AddWithValue("@Id", user.Id);
            command2.ExecuteNonQuery();

            user.Balance += amount;
            user.Transaction.Add(new Transaction
            {
              Id  = (int)command.LastInsertedId,
              CreatedAt = DateTime.Now,
              Type = Transaction.TransactionType.Deposit,
              Amount = amount,
              SenderAccountNumber = user.AccountNumber,
              ReciverAccountNumber = user.AccountNumber,
              BalanceAfter = user.Balance
            });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return false;
    }
    public bool UserWithdraw(User user, double amount)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        try
        {
            string updateBalanceQuery = "UPDATE users SET Balance = Balance + @amount WHERE Id = @Id";
            var command = new MySqlCommand(updateBalanceQuery, conn);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.ExecuteNonQuery();

            string insertTransactionQuery = "INSERT INTO transactions (CreatedAt, Type, Amount, SenderAccountNumber, ReciverAccountNumber, BalanceAfter, Id)" +
                                            " VALUES (@createdAt, @type, @amount, @senderAccountNumber, @reciverAccountNumber, @balanceAfter, @Id)";
            var command2 = new MySqlCommand(insertTransactionQuery, conn);
            command2.Parameters.AddWithValue("@createdAt", DateTime.Now);
            command2.Parameters.AddWithValue("@type", Transaction.TransactionType.Deposit.ToString());
            command2.Parameters.AddWithValue("@amount", amount);
            command2.Parameters.AddWithValue("@senderAccountNumber", user.AccountNumber);
            command2.Parameters.AddWithValue("@reciverAccountNumber", user.AccountNumber);
            command2.Parameters.AddWithValue("@balanceAfter", user.Balance - amount);
            command2.Parameters.AddWithValue("@Id", user.Id);
            command2.ExecuteNonQuery();
            if (amount <= user.Balance)
            {
                user.Balance -= amount;
                user.Transaction.Add(new Transaction
                {
                    Id  = (int)command.LastInsertedId,
                    CreatedAt = DateTime.Now,
                    Type = Transaction.TransactionType.Deposit,
                    Amount = amount,
                    SenderAccountNumber = user.AccountNumber,
                    ReciverAccountNumber = user.AccountNumber,
                    BalanceAfter = user.Balance
                });
            }
            else
            {
                Console.WriteLine("Withdraw Fail!!");
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return false;
    }

    public bool UserTransfer(User sender, string receiverAccountNumber, double amount)
    {
        var conn = new MySqlConnection(MySqlConnectionString);
        conn.Open();
        var transaction = conn.BeginTransaction();
        try
        {
            string findReceiverQuery = "SELECT * FROM users WHERE  AccountNumber = @receiveraccountNumber";
            var command = new MySqlCommand(findReceiverQuery, conn, transaction);
            command.Parameters.AddWithValue("@receiverAccountNumber", receiverAccountNumber);
            User receiver = null;
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                receiver = new User
                {
                    Id = reader.GetInt32("Id"),
                    UserName = reader.GetString("UserName"),
                    AccountNumber = reader.GetString("AccountNumber"),
                    FullName = reader.GetString("FullName"),
                    PhoneNumber = reader.GetString("PhoneNumber"),
                    Balance = reader.GetDouble("Balance"),
                    Status = reader.GetInt32("Status")
                };
            }

            if (receiver == null)
            {
                Console.WriteLine("Account Not Found");
            }

            if (sender.Balance < amount)
            {
                Console.WriteLine("Insufficient Balance");
            }
            //update sender Balance
            string updateSenderBalanceQuery = "UPDATE users SET Balance = Balance - @amount WHERE Id = @senderId";
            var updateSenderBalanceCommand = new MySqlCommand(updateSenderBalanceQuery, conn, transaction);
            updateSenderBalanceCommand.Parameters.AddWithValue("@amount", amount);
            updateSenderBalanceCommand.Parameters.AddWithValue("@senderId", sender.Id);
            updateSenderBalanceCommand.ExecuteNonQuery();
            //update receiver balance
            string updateReceiverBalanceQuery = "UPDATE users SET Balance = Balance + @amount WHERE Id = @receiverId";
            var updateReceiverBalanceCommand = new MySqlCommand(updateReceiverBalanceQuery, conn, transaction);
            updateReceiverBalanceCommand.Parameters.AddWithValue("@amount", amount);
            updateReceiverBalanceCommand.Parameters.AddWithValue("@receiverId", receiver.Id);
            updateReceiverBalanceCommand.ExecuteNonQuery();
            //save transaction
            string insertTransactionQuery = @"INSERT INTO transactions (CreatedAt, Type, Amount, SenderAccountNumber, ReciverAccountNumber, BalanceAfter, UserId) 
                                                      VALUES (@createdAt, @type, @amount, @senderAccountNumber, @receiverAccountNumber, @balanceAfter, @userId)";
            var insertTransactionCommand = new MySqlCommand(insertTransactionQuery, conn, transaction);
            insertTransactionCommand.Parameters.AddWithValue("@createdAt", DateTime.Now);
            insertTransactionCommand.Parameters.AddWithValue("@type", Transaction.TransactionType.Transfer.ToString());
            insertTransactionCommand.Parameters.AddWithValue("@amount", amount);
            insertTransactionCommand.Parameters.AddWithValue("@senderAccountNumber", sender.AccountNumber);
            insertTransactionCommand.Parameters.AddWithValue("@receiverAccountNumber", receiver.AccountNumber);
            insertTransactionCommand.Parameters.AddWithValue("@balanceAfter", sender.Balance - amount);
            insertTransactionCommand.Parameters.AddWithValue("@userId", sender.Id);
            insertTransactionCommand.ExecuteNonQuery();
            
            //update in database
            sender.Balance -= amount;
            receiver.Balance += amount;
            sender.Transaction.Add(new Transaction
            {
                Id = (int)insertTransactionCommand.LastInsertedId,
                CreatedAt = DateTime.Now,
                Type = Transaction.TransactionType.Transfer,
                Amount = amount,
                SenderAccountNumber = sender.AccountNumber,
                ReciverAccountNumber = receiver.AccountNumber,
                BalanceAfter = sender.Balance
            });

            receiver.Transaction.Add(new Transaction
            {
                Id = (int)insertTransactionCommand.LastInsertedId,
                CreatedAt = DateTime.Now,
                Type = Transaction.TransactionType.Transfer,
                Amount = amount,
                SenderAccountNumber = sender.AccountNumber,
                ReciverAccountNumber = receiver.AccountNumber,
                BalanceAfter = receiver.Balance
            });

            transaction.Commit();
            return true;
        }
        catch (Exception e)
        {
            transaction.Rollback();
            Console.WriteLine("Error" + e.Message);
            return false;


        }
    }
}