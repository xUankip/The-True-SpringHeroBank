using MySqlConnector;
using The_True_SpringHeroBank.Entity;

namespace The_True_SpringHeroBank.Repository;

public class TransactionRepository
{
    private const string MySqlConnectionString =
        "server=127.0.0.1;uid=root;" + "pwd=;database=the_true_spring_hero_bank";

    public bool UserDeposit(User user, double amount)
    {
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                // Update user balance
                var updateBalanceQuery = "UPDATE users SET Balance = Balance + @amount WHERE Id = @Id";
                var command = new MySqlCommand(updateBalanceQuery, conn, transaction);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.ExecuteNonQuery();

                // Insert transaction record
                var insertTransactionQuery =
                    "INSERT INTO transactions (CreatedAt, Type, Amount, SenderAccountNumber, ReciverAccountNumber, BalanceAfter, Id)" +
                    " VALUES (@createdAt, @type, @amount, @senderAccountNumber, @reciverAccountNumber, @balanceAfter, @Id)";
                var command2 = new MySqlCommand(insertTransactionQuery, conn, transaction);
                command2.Parameters.AddWithValue("@createdAt", DateTime.Now);
                command2.Parameters.AddWithValue("@type", Transaction.TransactionType.Deposit.ToString());
                command2.Parameters.AddWithValue("@amount", amount);
                command2.Parameters.AddWithValue("@senderAccountNumber", user.AccountNumber);
                command2.Parameters.AddWithValue("@reciverAccountNumber", user.AccountNumber);
                command2.Parameters.AddWithValue("@balanceAfter", user.Balance + amount);
                command2.ExecuteNonQuery();

                transaction.Commit();

                // Update user object
                user.Balance += amount;
                user.Transaction.Add(new Transaction
                {
                    CreatedAt = DateTime.Now,
                    Type = Transaction.TransactionType.Deposit,
                    Amount = amount,
                    SenderAccountNumber = user.AccountNumber,
                    ReciverAccountNumber = user.AccountNumber,
                    BalanceAfter = user.Balance
                });

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

    public bool UserWithdraw(User user, double amount)
    {
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                // Update user balance
                var updateBalanceQuery = "UPDATE users SET Balance = Balance - @amount WHERE Id = @Id";
                var command = new MySqlCommand(updateBalanceQuery, conn, transaction);
                command.Parameters.AddWithValue("@amount", amount);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.ExecuteNonQuery();

                // Insert transaction record
                var insertTransactionQuery =
                    "INSERT INTO transactions (CreatedAt, Type, Amount, SenderAccountNumber, ReciverAccountNumber, BalanceAfter, Id)" +
                    " VALUES (@createdAt, @type, @amount, @senderAccountNumber, @reciverAccountNumber, @balanceAfter, @Id)";
                var command2 = new MySqlCommand(insertTransactionQuery, conn, transaction);
                command2.Parameters.AddWithValue("@createdAt", DateTime.Now);
                command2.Parameters.AddWithValue("@type", Transaction.TransactionType.Withdraw.ToString());
                command2.Parameters.AddWithValue("@amount", amount);
                command2.Parameters.AddWithValue("@senderAccountNumber", user.AccountNumber);
                command2.Parameters.AddWithValue("@reciverAccountNumber", user.AccountNumber);
                command2.Parameters.AddWithValue("@balanceAfter", user.Balance + amount);
                command2.ExecuteNonQuery();

                transaction.Commit();

                // Update user object
                user.Balance -= amount;
                user.Transaction.Add(new Transaction
                {
                    CreatedAt = DateTime.Now,
                    Type = Transaction.TransactionType.Withdraw,
                    Amount = amount,
                    SenderAccountNumber = user.AccountNumber,
                    ReciverAccountNumber = user.AccountNumber,
                    BalanceAfter = user.Balance
                });

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

    public bool UserTransfer(User sender, string receiverAccountNumber, double amount)
    {
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var transaction = conn.BeginTransaction();
            try
            {
                // Find the receiver
                var findReceiverQuery = "SELECT * FROM users WHERE AccountNumber = @receiverAccountNumber";
                var findReceiverCommand = new MySqlCommand(findReceiverQuery, conn, transaction);
                findReceiverCommand.Parameters.AddWithValue("@receiverAccountNumber", receiverAccountNumber);
                User receiver = null;
                using (var reader = findReceiverCommand.ExecuteReader())
                {
                    if (reader.Read())
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
                    return false;
                }

                if (sender.Balance < amount)
                {
                    Console.WriteLine("Insufficient Balance");
                    return false;
                }

                // Update sender balance
                var updateSenderBalanceQuery = "UPDATE users SET Balance = Balance - @amount WHERE Id = @senderId";
                var updateSenderBalanceCommand = new MySqlCommand(updateSenderBalanceQuery, conn, transaction);
                updateSenderBalanceCommand.Parameters.AddWithValue("@amount", amount);
                updateSenderBalanceCommand.Parameters.AddWithValue("@senderId", sender.Id);
                updateSenderBalanceCommand.ExecuteNonQuery();

                // Update receiver balance
                var updateReceiverBalanceQuery = "UPDATE users SET Balance = Balance + @amount WHERE Id = @receiverId";
                var updateReceiverBalanceCommand = new MySqlCommand(updateReceiverBalanceQuery, conn, transaction);
                updateReceiverBalanceCommand.Parameters.AddWithValue("@amount", amount);
                updateReceiverBalanceCommand.Parameters.AddWithValue("@receiverId", receiver.Id);
                updateReceiverBalanceCommand.ExecuteNonQuery();

                // Save transaction
                var insertTransactionQuery =
                    @"INSERT INTO transactions (CreatedAt, Type, Amount, SenderAccountNumber, ReciverAccountNumber, BalanceAfter) 
                                              VALUES (@createdAt, @type, @amount, @senderAccountNumber, @receiverAccountNumber, @balanceAfter)";
                var insertTransactionCommand = new MySqlCommand(insertTransactionQuery, conn, transaction);
                insertTransactionCommand.Parameters.AddWithValue("@createdAt", DateTime.Now);
                insertTransactionCommand.Parameters.AddWithValue("@type",
                    Transaction.TransactionType.Transfer.ToString());
                insertTransactionCommand.Parameters.AddWithValue("@amount", amount);
                insertTransactionCommand.Parameters.AddWithValue("@senderAccountNumber", sender.AccountNumber);
                insertTransactionCommand.Parameters.AddWithValue("@receiverAccountNumber", receiver.AccountNumber);
                insertTransactionCommand.Parameters.AddWithValue("@balanceAfter", sender.Balance - amount);
                // insertTransactionCommand.Parameters.AddWithValue("@Id", sender.Id);
                insertTransactionCommand.ExecuteNonQuery();

                // Update sender and receiver objects
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
                conn.Close();
                return true;
            }
            catch (Exception e)
            {
                transaction.Rollback();
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
        }
    }

    public List<Transaction> FindAllTransactions()
    {
        var transactions = new List<Transaction>();
        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var query =
                "SELECT * FROM transactions";
            var command = new MySqlCommand(query, conn);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var transaction = new Transaction
                {
                    Id = reader.GetInt32("Id"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    Type = (Transaction.TransactionType)Enum.Parse(typeof(Transaction.TransactionType),
                        reader.GetString("Type")),
                    Amount = reader.GetDouble("Amount"),
                    SenderAccountNumber = reader.GetString("SenderAccountNumber"),
                    ReciverAccountNumber = reader.GetString("ReciverAccountNumber"),
                    BalanceAfter = reader.GetDouble("BalanceAfter")
                };
                transactions.Add(transaction);
            }

            return transactions;
        }
    }

    public List<Transaction> FindTransactionsByAccountNumber(string accountNumber)
    {
        var transactions = new List<Transaction>();

        using (var conn = new MySqlConnection(MySqlConnectionString))
        {
            conn.Open();
            var query =
                "SELECT * FROM transactions WHERE SenderAccountNumber = @accountNumber OR ReciverAccountNumber = @accountNumber";
            var command = new MySqlCommand(query, conn);
            command.Parameters.AddWithValue("@accountNumber", accountNumber);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var transaction = new Transaction
                {
                    Id = reader.GetInt32("Id"),
                    CreatedAt = reader.GetDateTime("CreatedAt"),
                    Type = (Transaction.TransactionType)Enum.Parse(typeof(Transaction.TransactionType),
                        reader.GetString("Type")),
                    Amount = reader.GetDouble("Amount"),
                    SenderAccountNumber = reader.GetString("SenderAccountNumber"),
                    ReciverAccountNumber = reader.GetString("ReciverAccountNumber"),
                    BalanceAfter = reader.GetDouble("BalanceAfter")
                };
                transactions.Add(transaction);
            }
        }

        return transactions;
    }
}