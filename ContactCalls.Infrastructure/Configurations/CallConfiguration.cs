using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCalls.Infrastructure.Configurations;

public class CallConfiguration : IEntityTypeConfiguration<Call>
{
    public void Configure(EntityTypeBuilder<Call> builder)
    {
        
        builder.HasKey(c => c.Id);
        
        builder.Property(c => c.StartTime)
            .IsRequired();
            
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>();
            
        builder.Property(c => c.Cost)
            .HasPrecision(10, 2);
            
        builder.HasOne(c => c.FromPhone)
            .WithMany(p => p.OutgoingCalls)
            .HasForeignKey(c => c.FromPhoneId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(c => c.ToPhone)
            .WithMany(p => p.IncomingCalls)
            .HasForeignKey(c => c.ToPhoneId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasIndex(c => c.FromPhoneId);
        builder.HasIndex(c => c.ToPhoneId);
        builder.HasIndex(c => c.StartTime);
        
        builder.HasCheckConstraint("CK_Call_Different_Phones", 
            "\"FromPhoneId\" != \"ToPhoneId\"");
    }
}
