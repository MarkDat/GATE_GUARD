namespace GATE_GUARD2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARKING")]
    public partial class PARKING
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(15)]
        public string TxtPlate { get; set; }

        public DateTime? DateTimeParkingIn { get; set; }

        public bool? Status { get; set; }

        public DateTime? DateTimeParkingOut { get; set; }

        public virtual LICENSE_PLATE LICENSE_PLATE { get; set; }

        public virtual USER_VEHICLE USER_VEHICLE { get; set; }
    }
}
