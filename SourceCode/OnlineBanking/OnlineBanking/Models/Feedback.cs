namespace OnlineBanking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Feedback")]
    public partial class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int FeedbackId { get; set; }

        public int CustomerId { get; set; }

        [Column(TypeName = "date")]
        public DateTime QuestionDate { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Question { get; set; }

        public int? AdminId { get; set; }

        public DateTime? AnswerDate { get; set; }

        [Column(TypeName = "text")]
        public string Answer { get; set; }

        public bool? Status { get; set; }

        public virtual Admintb Admintb { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
