namespace OnlineBanking.Models {
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OnlineBankingDbContext : DbContext {
        public OnlineBankingDbContext()
            : base("name=OnlineBankingDbContext") {
        }

        public virtual DbSet<Admintb> Admintbs { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<Cheque> Cheques { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<Admintb>()
                .Property(e => e.LoginName)
                .IsUnicode(false);

            modelBuilder.Entity<Admintb>()
                .Property(e => e.LoginPassword)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.AccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<BankAccount>()
                .Property(e => e.Balance)
                .HasPrecision(18, 3);

            modelBuilder.Entity<BankAccount>()
                .HasMany(e => e.Cheques)
                .WithRequired(e => e.BankAccount)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BankAccount>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.BankAccount)
                .HasForeignKey(e => e.SourceAccountNumber)
                .WillCascadeOnDelete(false);
            

            modelBuilder.Entity<Cheque>()
                .Property(e => e.AccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.LoginPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.TransactionPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.PhoneNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.BankAccounts)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.Question)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.Answer)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Question)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.Answer)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.SourceAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.TargetAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Amount)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.Balance)
                .HasPrecision(18, 3);
        }
    }
}
