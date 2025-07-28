using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCalls.Infrastructure.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(c => c.MiddleName)
            .HasMaxLength(100);
            
        builder.Property(c => c.CreatedAt);
            
        builder.HasOne(c => c.Profile)
            .WithOne(p => p.Contact)
            .HasForeignKey<ContactProfile>(p => p.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(c => c.Phones)
            .WithOne(p => p.Contact)
            .HasForeignKey(p => p.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
