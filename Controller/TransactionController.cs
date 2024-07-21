using The_True_SpringHeroBank.Entity;

namespace The_True_SpringHeroBank.Controller;

public class TransactionController
{
    public void DisplayTransaction(User user)
    {
        foreach (var transaction in user.Transaction)
        {
            Console.WriteLine("{0, -10} | {1, -20} | {2, -10} | {3, -15} | {4, -15} | {5, -10} | {6, -10}",
                transaction.Id, transaction.CreatedAt, transaction.Type, transaction.Amount, transaction.SenderAccountNumber, transaction.ReciverAccountNumber, transaction.BalanceAfter);
        }
    }

    public void DisplayTransactionByAccountNumber()
    {
        
    }
}