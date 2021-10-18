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
            BankService bankService = new BankService(); //creating a instance for service class
            List<string> LoginOptions = new List<string>() { "1.Create Bank Account", "2.Login to Account", "3.Exit" };
            Console.WriteLine("\nWelcome to XYZ bank ");
           

            int LoginOption = 0;       
            int ServiceOption = 0;
            string accountNumber = "";
            string bankName = "";
            string password = "";
            do
            {
                foreach (var d in LoginOptions)
                {
                    Console.WriteLine(d);
                }
                Console.WriteLine("Choose an option number");
                string ChoosedOption = Console.ReadLine();
                LoginOption = Convert.ToInt32(ChoosedOption);
                switch (ChoosedOption)
                {
                    case "1":
                        Console.WriteLine("");
                        Console.WriteLine("Enter Bank Name");
                         bankName = Console.ReadLine();
                        Console.Write("Enter Account Number: ");
                        accountNumber = Console.ReadLine();
                        Console.Write("Enter Your Account Password: ");
                        password = (Console.ReadLine());
                        bankService.BankCreation(bankName);
                        bankService.AccountCreation(bankName, accountNumber, password);
                        Console.WriteLine("Account Sucessfully Created");
                        break;

                    case "2":
                        Console.WriteLine("Enter Bank Name");
                          bankName = Console.ReadLine();
                        Console.Write("Enter Account Number: ");
                          accountNumber = Console.ReadLine();
                        Console.Write("Enter Your Account Password: ");
                          password = (Console.ReadLine());

                        if (bankService.IsBankPresent(bankName) && bankService.IsAccountPresent(bankName, accountNumber, password))
                        
                        {
                            do
                            {
                                secureMenu();
                                string MenuOption = Console.ReadLine();
                                ServiceOption = Convert.ToInt32(MenuOption);
                                switch (ServiceOption)
                                {
                                    case 1:
                                        int deposit_amount = 0;
                                        Console.Write("Enter the amount to deposit : rs ");
                                        deposit_amount = Convert.ToInt32(Console.ReadLine());
                                        if (bankService.Deposit(bankName,accountNumber, deposit_amount))
                                        {
                                            Console.WriteLine("Amount sucessfully deposited to your Account");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid Account details");
                                        }
                                        break;
                                    case 2:
                                        int withdraw_amount = 0;
                                        Console.Write("Enter amount to withdraw: rs ");
                                        withdraw_amount = Convert.ToInt32(Console.ReadLine());
                                        if (bankService.Withdraw(bankName,accountNumber, withdraw_amount))
                                        {
                                            Console.WriteLine("Amount sucessfully withdrawn , Please collect your money ");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Insufficient funds , please enter valid amount ");
                                        }
                                        break;
                                    case 3:
                                        
                                        List<Transactions>Transactions= bankService.TransactionLog(bankName, accountNumber);
                                        foreach (var d in Transactions)
                                        {
                                            Console.WriteLine(d.Id + " " + d.Type + " " + d.Amount+" "+ d.On);
                                        }
                                        break;

                                        
                                    case 4:
                                        //Console.WriteLine("Enter Your Bank Name : ");
                                        //string SourceBankName = Console.ReadLine();
                                        //Console.WriteLine("enter Your account Number : ");
                                        //string SourceAccountNumber = Console.ReadLine();
                                        Console.WriteLine("Enter the amount of money to be transferred : ");
                                        int transfered_money = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Enter Destination Bank Name : ");
                                        string DestinationBankName = Console.ReadLine();
                                        Console.WriteLine("enter Destination Account Number  : ");
                                        string DestinationAccountNumber = Console.ReadLine();

                                        if (bankService.Transfer(bankName,accountNumber,transfered_money,DestinationBankName,DestinationAccountNumber))
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
                    case "3":
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
