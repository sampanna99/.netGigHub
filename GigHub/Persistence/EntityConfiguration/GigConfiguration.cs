using GigHub.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistence.EntityConfiguration
{
    public class GigConfiguration : EntityTypeConfiguration<Gig>
    {
        public GigConfiguration()
        {
            Property(g => g.ArtistId)
                .IsRequired();

            Property(g => g.GenreId)
                .IsRequired();

            Property(g => g.Venue)
                .IsRequired()
                .HasMaxLength(255);

            //modelBuilder.Entity<Attendance>() implementation below

            //   .HasRequired(a => a.Gig)
            //   .WithMany(g => g.Attendances)
            //   .WillCascadeOnDelete(false);


            HasMany(g => g.Attendances)
                .WithRequired(g => g.Gig)
                .WillCascadeOnDelete(false);


        }
    }
}