namespace The_True_SpringHeroBank.Entity;

public class Transaction()
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public TransactionType Type { get; set; }
    public double Amount { get; set; }
    public string SenderAccountNumber { get; set; }
    public string ReciverAccountNumber { get; set; }
    public double BalanceAfter { get; set; }
    public enum TransactionType
    {
        Deposit, Withdraw, Transfer
    }
}