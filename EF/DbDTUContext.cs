namespace GATE_GUARD2.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DbDTUContext : DbContext
    {
        public DbDTUContext()
            : base("name=DbDTUContext")
        {
        }

        public virtual DbSet<LICENSE_PLATE> LICENSE_PLATE { get; set; }
        public virtual DbSet<USER_VEHICLE> USER_VEHICLE { get; set; }
        public virtual DbSet<PARKING> PARKINGs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LICENSE_PLATE>()
                .Property(e => e.TxtPlate)
                .IsUnicode(false);

            modelBuilder.Entity<USER_VEHICLE>()
                .Property(e => e.IDS)
                .IsFixedLength();

            modelBuilder.Entity<USER_VEHICLE>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<PARKING>()
                .Property(e => e.ID)
                .IsUnicode(false);

            modelBuilder.Entity<PARKING>()
                .Property(e => e.TxtPlate)
                .IsUnicode(false);
        }
    }
}
