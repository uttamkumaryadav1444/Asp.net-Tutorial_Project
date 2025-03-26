using EventManagementWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventManagementWebApp.Data.Configurations
{
    public class RegistrationConfigurations : IEntityTypeConfiguration<Registration>
    {
        public void Configure(EntityTypeBuilder<Registration> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Event)
                   .WithMany(e => e.Registrations)
                   .HasForeignKey(x => x.EventId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.HasOne(x => x.Member)
                   .WithMany(m => m.Registrations)
                   .HasForeignKey(x => x.MemberId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(x => x.Tickets)
                   .IsRequired();
        }
    }
}
