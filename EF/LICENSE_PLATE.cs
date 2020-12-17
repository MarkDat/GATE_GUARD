namespace GATE_GUARD2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LICENSE_PLATE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LICENSE_PLATE()
        {
            PARKINGs = new HashSet<PARKING>();
        }

        public string ImgPath { get; set; }

        public string ImgPlatePath { get; set; }

        [Key]
        [StringLength(15)]
        public string TxtPlate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PARKING> PARKINGs { get; set; }
    }
}
