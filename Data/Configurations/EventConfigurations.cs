using EventManagementWebApp.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace EventManagementWebApp.Data.Configurations
{
    public class EventConfigurations : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Organizer)
                   .WithMany(o => o.CreatedEvents)
                   .HasForeignKey(x => x.OrganizerId)
                   .OnDelete(DeleteBehavior.Restrict); 


            builder.HasMany(x => x.Registrations)
                   .WithOne(r => r.Event)
                   .HasForeignKey(r => r.EventId)
                   .OnDelete(DeleteBehavior.Restrict); 

            builder.Property(x => x.Name)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            builder.Property(x => x.Date)
                   .IsRequired();

            builder.Property(x => x.Location)
                   .HasMaxLength(300)
                   .IsRequired();
        }
    }

}
