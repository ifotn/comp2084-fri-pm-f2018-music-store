namespace fri_pm_music_store.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        [StringLength(256)]
       
        public string Username { get; set; }

        [StringLength(160)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(160)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(70)]
        [Required]
        public string Address { get; set; }

        [StringLength(40)]
        [Required]
        public string City { get; set; }

        [StringLength(40)]
        [Required]
        public string State { get; set; }

        [StringLength(10)]
        [Required]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [StringLength(40)]
        [Required]
        public string Country { get; set; }

        [StringLength(24)]
        public string Phone { get; set; }

        [StringLength(160)]
        public string Email { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Total { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
