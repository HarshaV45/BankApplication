using System;
using System.Collections.Generic;
using System.Text;
using TechnovertAtm.CLI.Enums;
using TechnovertAtm.Services;
using TechonovertAtm.Models;
using TechonovertAtm.Models.Enums;

namespace TechnovertAtm.CLI
{
    public class CustomerConsole
    {
        public void Login(BankServices bankService, CustomerService customerService, TransactionService transactionService)
        {
            try
            {
                string accountId = "";
                string bankId = "";
                string password = "";
                Console.Write("Enter your BankId: ");
                bankId = Console.ReadLine();
                Console.Write("Enter your Account Id : ");
                accountId = Console.ReadLine();
            
                bankService.BankChecker(bankId);

                Console.Write("Enter your Password : ");
                password = Console.ReadLine();
                customerService.AccountLogin(bankId, accountId, password);


                string[] customerOptions = { "1.Deposit", "2.Withdraw", "3.Transfer", "4.Transaction History", "5.View Balance", "6.LogOut" };
                int choosedOption = 0;

                bool stop = true;
                do
                {
                   
                    foreach (var d in customerOptions)
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
                        case (int)UserOptions.Deposit:
                            decimal deposit_amount = 0;

                            Console.Write("Enter the Currency Code : ");
                            string code = Console.ReadLine();

                            Console.Write("Enter the amount to deposit :  ");
                            deposit_amount = Convert.ToDecimal(Console.ReadLine());
                            try
                            {
                                transactionService.Deposit(bankId, accountId, deposit_amount, code);
                                Console.WriteLine("Amount sucessfully deposited to your Account");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case (int)UserOptions.Withdraw:
                            decimal withdraw_amount = 0;

                            Console.Write("Enter amount to withdraw: rs ");
                            withdraw_amount = Convert.ToDecimal(Console.ReadLine());
                            try
                            {
                                transactionService.Withdraw(bankId, accountId, withdraw_amount);

                                Console.WriteLine("Amount sucessfully withdrawn , Please collect your money ");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case (int)UserOptions.Transfer:

                            Console.WriteLine("Enter the amount of money to be transferred : ");
                            decimal transfered_money = Convert.ToDecimal(Console.ReadLine());
                            Console.WriteLine("Enter Destination Bank Name : ");
                            string DestinationBankId = Console.ReadLine();
                            Console.WriteLine("enter Destination Account Number  : ");
                            string DestinationAccountId = Console.ReadLine();

                            Console.WriteLine("Available Tax Charges for Transaction are : ");
                            Console.WriteLine("\n1. IMPS\n2. RTGS");

                            Console.Write("\nEnter the Tax Type in which you would like to transfer the money : ");
                           
                            string UserTaxType = Console.ReadLine();
                            
                            try
                            {
                                transactionService.Transfer(bankId, accountId,transfered_money,DestinationBankId,DestinationAccountId,UserTaxType );

                                Console.WriteLine("Amount sucessfully Transfered to destination Account ");

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case (int)UserOptions.TransactionHistory:
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
                        case (int)UserOptions.Balance:
                            try
                            {
                                decimal balance = transactionService.ViewBalance(bankId, accountId);
                                Console.WriteLine("Available Balance is : " + balance);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                            break;
                        case (int)UserOptions.LogOut:
                            Console.WriteLine("Logged Out");
                            stop = false;
                            break;
                    }


                } while (stop);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
