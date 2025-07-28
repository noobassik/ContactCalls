using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCalls.Infrastructure.Configurations;

public class ConferenceParticipantConfiguration : IEntityTypeConfiguration<ConferenceParticipant>
{
    public void Configure(EntityTypeBuilder<ConferenceParticipant> builder)
    {
        
        builder.HasKey(cp => cp.Id);
        
        builder.Property(cp => cp.JoinTime)
            .IsRequired();
            
        builder.HasOne(cp => cp.Phone)
            .WithMany(p => p.ConferenceParticipations)
            .HasForeignKey(cp => cp.PhoneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}