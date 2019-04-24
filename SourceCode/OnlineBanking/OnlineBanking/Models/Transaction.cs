namespace OnlineBanking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction")]
    public partial class Transaction
    {
        public int TransactionId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(30)]
        public string SourceAccountNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string TargetAccountNumber { get; set; }

        public DateTime Date { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public bool SendReceiveStatus { get; set; }

        public decimal Amount { get; set; }

        public decimal? Balance { get; set; }

        public virtual BankAccount BankAccount { get; set; }
        
        public virtual Customer Customer { get; set; }
    }
}
