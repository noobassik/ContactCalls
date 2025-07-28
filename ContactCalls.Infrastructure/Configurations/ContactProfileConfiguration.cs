using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCalls.Infrastructure.Configurations;

public class ContactProfileConfiguration : IEntityTypeConfiguration<ContactProfile>
{
    public void Configure(EntityTypeBuilder<ContactProfile> builder)
    {
        builder.HasKey(cp => cp.Id);
        
        builder.Property(cp => cp.Email)
            .HasMaxLength(255);
            
        builder.Property(cp => cp.Address)
            .HasMaxLength(500);
            
        builder.Property(cp => cp.Company)
            .HasMaxLength(200);
            
        builder.Property(cp => cp.Position)
            .HasMaxLength(200);
            
        builder.Property(cp => cp.Notes)
            .HasMaxLength(2000);
            
        builder.HasIndex(cp => cp.ContactId)
            .IsUnique();
    }
}
