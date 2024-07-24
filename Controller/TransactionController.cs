using The_True_SpringHeroBank.Repository;

namespace The_True_SpringHeroBank.Controller;

public class TransactionController
{
    public void DisplayTransaction()
    {
        var _transactionRepository = new TransactionRepository();
        Console.WriteLine("{0, -10} | {1, -20} | {2, -10} | {3, -10} | {4, -20} | {5, -20} | {6, -10}",
            "Id", "CreatedAt", "Type", "Amount", "SenderAccount", "ReceiverAccount", "BalanceAfter");
        var transactions = _transactionRepository.FindAllTransactions();
        foreach (var transaction in transactions)
            Console.WriteLine("{0, -10} | {1, -20} | {2, -10} | {3, -15} | {4, -15} | {5, -10} | {6, -10}",
                transaction.Id, transaction.CreatedAt, transaction.Type, transaction.Amount,
                transaction.SenderAccountNumber, transaction.ReciverAccountNumber, transaction.BalanceAfter);
    }

    public void DisplayTransactionByAccountNumber()
    {
        var _transactionRepository = new TransactionRepository();
        Console.WriteLine("Enter your Account Number:");
        var accountNumber = Console.ReadLine();

        var transactions = _transactionRepository.FindTransactionsByAccountNumber(accountNumber);

        if (transactions.Count == 0)
        {
            Console.WriteLine("No transactions found for this account.");
            return;
        }

        Console.WriteLine("{0, -10} | {1, -20} | {2, -10} | {3, -10} | {4, -20} | {5, -20} | {6, -10}",
            "Id", "CreatedAt", "Type", "Amount", "SenderAccount", "ReceiverAccount", "BalanceAfter");

        foreach (var transaction in transactions)
            Console.WriteLine("{0, -10} | {1, -20} | {2, -10} | {3, -10} | {4, -20} | {5, -20} | {6, -10}",
                transaction.Id, transaction.CreatedAt, transaction.Type, transaction.Amount,
                transaction.SenderAccountNumber, transaction.ReciverAccountNumber, transaction.BalanceAfter);
    }
}