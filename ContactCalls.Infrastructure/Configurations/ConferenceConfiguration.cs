using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCalls.Infrastructure.Configurations;

public class ConferenceConfiguration : IEntityTypeConfiguration<Conference>
{
    public void Configure(EntityTypeBuilder<Conference> builder)
    {
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(200);
            
        builder.Property(c => c.StartTime)
            .IsRequired();
            
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>();
            
        builder.HasMany(c => c.Participants)
            .WithOne(p => p.Conference)
            .HasForeignKey(p => p.ConferenceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
