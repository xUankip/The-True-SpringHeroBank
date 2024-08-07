﻿namespace The_True_SpringHeroBank.Entity;

public class User
{
    public enum UserType
    {
        RegularUser,
        Admin
    }

    public int Id { get; set; }
    public string UserName { get; set; }
    public string PassWord { get; set; }
    public string FullName { get; set; }
    public string AccountNumber { get; set; }
    public string PhoneNumber { get; set; }
    public double Balance { get; set; } = 0;
    public List<Transaction> Transaction { get; set; } = new();
    public UserType Type { get; set; }
    public int Status { get; set; } //1-active 0-lock
}