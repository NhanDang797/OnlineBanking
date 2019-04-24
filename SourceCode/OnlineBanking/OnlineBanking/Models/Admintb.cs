namespace OnlineBanking.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Admintb")]
    public partial class Admintb
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Admintb()
        {
            Feedbacks = new HashSet<Feedback>();
        }

        [Key]
        public int AdminId { get; set; }

        [Required]
        [StringLength(255)]
        public string LoginName { get; set; }

        [Required]
        [StringLength(255)]
        public string LoginPassword { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
    }
}
