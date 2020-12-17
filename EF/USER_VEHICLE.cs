namespace GATE_GUARD2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_VEHICLE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER_VEHICLE()
        {
            PARKINGs = new HashSet<PARKING>();
        }

        [StringLength(20)]
        public string IDS { get; set; }

        [StringLength(50)]
        public string ID { get; set; }

        [StringLength(30)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARKING> PARKINGs { get; set; }
    }
}
