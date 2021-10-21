using System;
using System.Collections.Generic;
using TechnovertAtm.Services;
using TechonovertAtm.Models;

namespace TechnovertAtm.CLI
{
    public class Program
    {

       
        public static void secureMenu()
        {
            Console.WriteLine("|----------------------------|");
            Console.WriteLine("|xyz bank ATM secure menu    |");
            Console.WriteLine("|                            |");
            Console.WriteLine("| 1.Deposit                  |");
            Console.WriteLine("| 2.withdraw                 |");
            Console.WriteLine("| 3.Transaction Log          |");
            Console.WriteLine("| 4.Transfer                 |");
            Console.WriteLine("| 5.Logout                   |");
            Console.WriteLine("|                            |");
            Console.WriteLine("|----------------------------|");
            Console.WriteLine("                              ");
            Console.Write("Enter your option:     ");



        }

        static void Main(string[] args)
        {
            BankService bankService = new BankService(); 
            List<string> LoginOptions = new List<string>() { "1.Create Bank Account", "2.Login to Account", "3.Exit" };
            Console.WriteLine("\nWelcome to XYZ bank ");
           

            int LoginOption = 0;       
            int ServiceOption = 0;
            string accountName = "";
            string bankName = "";
            string password = "";
            do
            {
                foreach (var d in LoginOptions)
                {
                    Console.WriteLine(d);
                }
                try
                {
                    Console.WriteLine("Choose an option number");
                    string ChoosedOption = Console.ReadLine();
                    LoginOption = Convert.ToInt32(ChoosedOption);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                switch (LoginOption)
                {
                 
                    case 1:
                        try
                        {
                            Console.WriteLine("");
                            Console.WriteLine("Enter Bank Name");
                             bankName = Console.ReadLine();
                            Console.Write("Enter Account Holder Name: ");
                             accountName = Console.ReadLine();
                            Console.Write("Enter Your Account Password: ");
                            password = (Console.ReadLine());
                            bankService.BankCreation(bankName);
                            bankService.AccountCreation(bankName, accountName, password);
                            Console.WriteLine("Account Sucessfully Created");
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    break;

                    case 2:
                        Console.WriteLine("Enter Bank Name");
                    bankName = Console.ReadLine();
                    Console.Write("Enter Account Holder Name: ");
                    accountName = Console.ReadLine();
                    Console.Write("Enter Your Account Password: ");
                    password = (Console.ReadLine());
                
                         if (bankService.BankLogin(bankName) && bankService.AccountLogin(bankName, accountName, password))
                            {      
                            do
                            {
                                secureMenu();
                                try
                                {
                                    string MenuOption = Console.ReadLine();
                                    ServiceOption = Convert.ToInt32(MenuOption);
                                }
                                catch(Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                switch (ServiceOption)
                                {
                                    case 1:
                                        int deposit_amount = 0;
                                        Console.Write("Enter the amount to deposit : rs ");
                                        deposit_amount = Convert.ToInt32(Console.ReadLine());
                                        try
                                        {
                                            bankService.Deposit(bankName, accountName, deposit_amount);
                                            Console.WriteLine("Amount sucessfully deposited to your Account");
                                            
                                        }
                                        catch(Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                        break;
                                    case 2:
                                        int withdraw_amount = 0;
                                        Console.Write("Enter amount to withdraw: rs ");
                                        withdraw_amount = Convert.ToInt32(Console.ReadLine());
                                        try
                                        {
                                            bankService.Withdraw(bankName, accountName, withdraw_amount);

                                            Console.WriteLine("Amount sucessfully withdrawn , Please collect your money ");
                                        }
                                        catch(Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        } 
                                        break;
                                    case 3:
                                        
                                        List<Tranaction>Transactions= bankService.TransactionLog(bankName, accountName);
                                        foreach (var d in Transactions)
                                        {
                                            Console.WriteLine(d.Id + " " + d.Type + " " + d.Amount+" "+ d.On);
                                        }
                                        break;

                                        
                                    case 4:
                                       
                                        Console.WriteLine("Enter the amount of money to be transferred : ");
                                        int transfered_money = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Enter Destination Bank Name : ");
                                        string DestinationBankName = Console.ReadLine();
                                        Console.WriteLine("enter Destination Account Number  : ");
                                        string DestinationAccountNumber = Console.ReadLine();

                                        if (bankService.Transfer(bankName, accountName, transfered_money,DestinationBankName,DestinationAccountNumber))
                                        {
                                            Console.WriteLine("------ Amount sucessfully transferred -------");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Please enter valid details");
                                        }
                                        break;
                                    case 5:
                                        Console.WriteLine("Logout sucessful");
                                        break;
                                    default:
                                        Console.WriteLine("Invalid option enterned");
                                        break;
                                }
                            } while (ServiceOption != 5);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Details ,Please enter details Correctly  ");
                        }

                        break;
                    case 3:
                        break;

                    default:
                        Console.WriteLine("Invalid option entered");
                        break;

                }
            } while (LoginOption != 3);
            Console.WriteLine("Thanks for using xyz bank");

        }

    }
}
