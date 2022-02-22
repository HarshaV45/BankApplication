using System;
using System.Collections.Generic;
using System.Text;
using TechnovertAtm.Services;
using TechonovertAtm.Models;
using TechnovertAtm.CLI.Enums;

namespace TechnovertAtm.CLI
{
    public class StaffConsole
    {
        public void Login(BankServices bankService,AccountServices customerService,StaffService staffService,TransactionService transactionService)
        {
            try
            {
                string StaffId, password;
                Console.Write("Enter Staff ID : ");
                StaffId = Console.ReadLine();
                Console.Write("Enter your password: ");
                password = Console.ReadLine();

                //staffService.Login( bankService, customerService, transactionService, StaffId, password);
           
            string[] staffOptions = { "1.Create new Account", "2.Update Account", "3.Delete Account", "4.Customer Transaction History", "5.Revert Transaction", "6.Add new Currency", "7.View Account Details", "8.Logout" };
            int choosedOption = 0;
                bool stop = true;
            do
            {


                foreach (var d in staffOptions)
                {
                    Console.WriteLine(d);
                }
                try
                {
                    Console.Write("Enter Option Number : ");
                    
                    choosedOption = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                switch (choosedOption)
                {
                    case (int)StaffOptions.NewAccount:

                        string name, bankName;
                        int age = 0;
                        Console.Write("Enter the Account Holder name :");
                        name = Console.ReadLine();
                        Console.Write("Enter Bank Id : ");
                        bankName = Console.ReadLine();
                        Console.Write("Enter the Age : ");
                        string tempAge = Console.ReadLine();
                        if (String.IsNullOrEmpty(tempAge) == false)
                        {
                          age = Convert.ToInt32(tempAge);
                        }
                        try
                        {
                            string[] details = staffService.CreateAccount(name, bankName,age);
                            string newId = details[0], accountpassword = details[1];

                            Console.WriteLine("New Account Credintials are : ");
                            Console.WriteLine("\nAccount Id : " + newId);
                            Console.WriteLine("Password is : " + accountpassword);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case (int)StaffOptions.UpdateAccount:

                        string accountId, bankId;
                        Console.Write("Enter Your Bank Id : ");
                        bankId = Console.ReadLine();
                        Console.Write("Enter Your Account Id :");
                        accountId = Console.ReadLine();
                        string[] updateOptions = { "1.Update Name", "2.Update Password" };
                        foreach (var d in updateOptions)
                        {
                            Console.WriteLine(d);
                        }
                        string updateOption = Console.ReadLine();
                        string newName, newPassword;
                        switch (updateOption)
                        {
                            case "1":
                                Console.Write("Enter the new Name : ");
                                newName = Console.ReadLine();
                                try
                                {
                                    staffService.UpdateAccount(accountId, bankId, newName, null); //check
                                    Console.WriteLine("Name Updated");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                            case "2":
                                Console.Write("Enter the new Password : ");
                                newPassword = Console.ReadLine();
                                try
                                {
                                    staffService.UpdateAccount(accountId, bankId, null, newPassword);
                                    Console.WriteLine("Password Updated");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                break;
                        }
                        break;
                    case (int)StaffOptions.DeleteAccount:
                        string deleteAccountId, deleteBankId;
                        Console.Write("Enter the BankId : ");
                        deleteBankId = Console.ReadLine();
                        Console.Write("Enter the Account Id : ");
                        deleteAccountId = Console.ReadLine();
                        try
                        {
                            staffService.DeleteAccount(deleteAccountId, deleteBankId);
                            Console.WriteLine("Account sucessfully deleted");

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case (int)StaffOptions.TransactionHistory:
                        Console.Write("Enter the bankId : ");
                        bankId = Console.ReadLine();
                        Console.Write("Enter the account Id : ");
                        accountId = Console.ReadLine();
                        try
                        {
                            List<Transaction> transactions = transactionService.TransactionLog(bankId, accountId);
                            Console.WriteLine("Transaction Log");
                            foreach (var transaction in transactions)
                            {
                               Console.WriteLine(transaction.Id + "  " + transaction.Amount + "  " + transaction.Type + "  " + transaction.On);
                           }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    case (int)StaffOptions.RevertTransaction:
                        Console.Write("Enter the Bank Id in which the account exists : ");
                        bankId = Console.ReadLine();
                        Console.Write("Enter the Account Id : ");
                        accountId = Console.ReadLine();
                        try
                        {
                            List<Transaction> transaction = transactionService.TransactionLog(bankId, accountId);
                            string transactionId = Console.ReadLine();
                            staffService.RevertTransaction(bankId, accountId, transactionId);
                            Console.WriteLine("Transaction has been reverted");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                        case (int)StaffOptions.NewCurrency:
                            string currencyName, currencyCode;
                            decimal currencyExchangeRate;
                            Console.Write("Enter the name of new Currency : ");
                            currencyName = Console.ReadLine();
                            Console.Write("Enter the new currency code : ");
                            currencyCode = Console.ReadLine();
                            Console.Write("Enter the Exchange rate with respect to INR : ");
                            currencyExchangeRate = Convert.ToInt32(Console.ReadLine());
                            staffService.AddNewCurrency(currencyName, currencyCode, currencyExchangeRate);
                            Console.WriteLine("New Currency sucessfully Added");

                            break;
                    case (int)StaffOptions.AccountDetails:
                        Console.Write("Enter the Bank Id : ");
                        bankId = Console.ReadLine();
                        Console.Write("Enter the Account Id : ");
                        accountId = Console.ReadLine();
                        BankAccount bankAccount = staffService.viewAccountDetails(bankId, accountId);
                        Console.WriteLine("\nName : " + bankAccount.Name);
                        Console.WriteLine("Id : " + bankAccount.AccountId);
                        Console.WriteLine("Password : " + bankAccount.Password);
                        Console.WriteLine("Balance : " + bankAccount.Amount);
                        break;
                    case (int)StaffOptions.Logout:
                        Console.WriteLine("Logged Out");
                            stop = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option entered");
                        break;

                }
            } while (stop);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }


        }
    }
}
