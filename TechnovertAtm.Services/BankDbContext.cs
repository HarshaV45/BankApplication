using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TechonovertAtm.Models;

namespace TechnovertAtm.Services
{
    public class BankDbContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Currency> Curriencies { get; set; }
        public DbSet<StaffAccount> Staff { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options)
        {

        }

        /* protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
         {
             optionsBuilder.UseSqlServer("Data Source=LAPTOP-624M6E7F;Initial Catalog=AtmDatabase;Integrated Security=True");
         }*/

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Banks");
                entity.Property(m => m.BankId);
                entity.Property(m => m.Name);
                entity.Property(m => m.Description);
                entity.Property(m => m.BankStatus);

            });
            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.ToTable("BankAccounts");
                entity.Property(m => m.AccountId);
                entity.Property(m => m.Name);
                entity.Property(m => m.Password);
                entity.Property(m => m.Amount);
                entity.Property(m => m.BankId);
                entity.Property(m => m.Gender);
                entity.Property(m => m.AccountStatus);
                entity.Property(m => m.Role);
            });

            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currencies");
                entity.Property(m => m.CurrencyCode);
                entity.Property(m => m.CurrencyName);
                entity.Property(m => m.CurrencyExchangeRate);
            });

            modelBuilder.Entity<StaffAccount>(entity =>
            {
                entity.ToTable("StaffAccounts");
                entity.Property(m => m.Id);
                entity.Property(m => m.Name);
                entity.Property(m => m.Password);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transactions");
                entity.Property(m => m.Id);
                entity.Property(m => m.BankId);
                entity.Property(m => m.AccountId);
                entity.Property(m => m.Amount);
                entity.Property(m => m.Tax);
                entity.Property(m => m.TaxType);
                entity.Property(m => m.DestinationBankId);
                entity.Property(m => m.DestinationAccountId);
                entity.Property(m => m.TransactionType);
                entity.Property(m => m.On);
            });


        }
    }
}
 