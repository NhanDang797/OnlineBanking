namespace OnlineBanking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FAQ")]
    public partial class FAQ
    {
        public int Id { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Question { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Answer { get; set; }
    }
}
