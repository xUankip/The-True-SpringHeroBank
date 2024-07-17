using System.Transactions;

namespace The_True_SpringHeroBank.Entity;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string FullName { get; set; }
    public string AccountNumber { get; set; }
    public string PhoneNumber { get; set; }
    public double Balance { get; set; } = 0;
    public List<Transaction> Transaction { get; set; } = new List<Transaction>();
    public UserType Type { get; set; }
    public int Status { get; set; } //1-active 2-lock
    public enum UserType
    {
        RegularUser, Admin
    }
}