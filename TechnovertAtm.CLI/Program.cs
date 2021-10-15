using System;
using System.Collections.Generic;
using TechnovertAtm.Services;

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
            List<string> LoginOptions = new List<string>() { "1.Login", "2.Exit" };
            Console.WriteLine("Welcome to xyz Bank");
           

            int LoginOption = 0;       
            int ServiceOption = 0;
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
                        Console.Write("Enter Account Number: ");
                        int cardNo = Convert.ToInt32(Console.ReadLine());
                        Console.Write("Enter 6 digits pin: ");
                        int pin = Convert.ToInt32(Console.ReadLine());

                        if (bankService.validateCardDetails(cardNo, pin) == false)//using the same instance for all methods
                        {
                            Console.WriteLine("These Details are not present in the Bank");
                            Console.WriteLine("Would you like to Create a new Account using these details: Yes/No");
                            string newAccountOption = Console.ReadLine();
                            if (newAccountOption.ToLower() == "yes")
                            {
                                bankService.AddAccount(cardNo, pin);
                            }
                        }
                        else
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
                                        if (bankService.deposit(cardNo, deposit_amount))
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
                                        if (bankService.withdraw(cardNo, withdraw_amount))
                                        {
                                            Console.WriteLine("Amount sucessfully withdrawn , Please collect your money ");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Insufficient funds , please enter valid amount ");
                                        }
                                        break;
                                    case 3:
                                        List<string> log = bankService.TransactionLog(cardNo);
                                        foreach (var d in log)
                                        {
                                            Console.WriteLine(d);
                                        }
                                        break;
                                    case 4:
                                        Console.WriteLine("Enter account number");
                                        int accnum = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("enter account number to be transferred");
                                        int accnum1 = Convert.ToInt32(Console.ReadLine());
                                        Console.WriteLine("Enter the amount of money to be transferred : ");
                                        int transfered_money = Convert.ToInt32(Console.ReadLine());
                                        if (bankService.transfer(accnum, accnum1, transfered_money))
                                        {
                                            Console.WriteLine("Amount sucessfully transferred");
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

                        break;
                    case "2":
                        break;

                    default:
                        Console.WriteLine("Invalid option entered");
                        break;

                }
            } while (LoginOption != 2);
            Console.WriteLine("Thanks for using xyz bank");

        }

    }
}
