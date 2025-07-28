using ContactCalls.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContactCalls.Infrastructure.Configurations;

public class PhoneConfiguration : IEntityTypeConfiguration<Phone>
{
	public void Configure(EntityTypeBuilder<Phone> builder)
	{

		builder.HasKey(p => p.Id);

		builder.Property(p => p.Number)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(p => p.Description)
			.HasMaxLength(200);

		builder.Property(p => p.CreatedAt);

		builder.HasIndex(p => p.Number)
			.IsUnique();

		builder.HasIndex(p => p.ContactId);

		builder.HasCheckConstraint("CK_Phone_Number_Format",
			"\"Number\" ~ '^\\+[0-9]-[0-9]{3}-[0-9]{3}-[0-9]{2}-[0-9]{2}$'");
	}
}