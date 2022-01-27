﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using TechnovertAtm.CLI.Enums;
using TechnovertAtm.Services;
using TechonovertAtm.Models;

namespace TechnovertAtm.CLI
{
    public class Program
    {
        static void Main()
        {
            BankDbContext DbContext = new BankDbContext();
            StaffService staffService = new StaffService(DbContext);
            CurrencyExchanger currencyExchanger = new CurrencyExchanger();
            BankServices bankService = new BankServices(DbContext);
            CustomerService customerService = new CustomerService(DbContext,bankService);
            TransactionService transactionService = new TransactionService(DbContext, customerService, currencyExchanger);
            try
            {

               // staffService.CreateStaffAccount("Admin"); //Creates default staff as Admin and defaulat password is STA+Name+@123
               //// currencyExchanger.CurrencyExchange(); // Adds the default accepted currencies into the Application


               // bankService.BankCreation("SBI"); // Default banks and defalult BankId is BankName+123
               // bankService.BankCreation("Axis");
               // bankService.BankCreation("HDFC");

                string[] loginOptions = { "1.Account Holder Login", "2.Staff Login", "3.Exit from Application" };
                int choosedOption = 0;
                bool stop = true;
                do
                {
                    foreach (var d in loginOptions)
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
                        case (int)LoginOptions.AccountHolder:
                            CustomerConsole accountHolder = new CustomerConsole();
                            accountHolder.Login(bankService, customerService, transactionService);
                            break;

                        case (int)LoginOptions.StaffLogin:
                            StaffConsole staff = new StaffConsole();
                            staff.Login(bankService, customerService, staffService, transactionService);
                            break;


                        case (int)LoginOptions.Exit:
                            stop = false;
                            Console.WriteLine("Exited Sucessfully");
                            break;
                        default:
                            Console.WriteLine("Invalid Option Entered") ;
                            break;

                    }
                } while (stop);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

