namespace OnlineBanking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cheque")]
    public partial class Cheque
    {
        public int ChequeID { get; set; }

        [Required]
        [StringLength(30)]
        public string AccountNumber { get; set; }

        public DateTime? IssuedDate { get; set; }

        public DateTime? EndDate { get; set; }

        public short NumberOfChequeBook { get; set; }

        public bool? Status { get; set; }

        public virtual BankAccount BankAccount { get; set; }
    }
}
