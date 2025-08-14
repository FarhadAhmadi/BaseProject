using BaseProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BaseProject.Infrastructure.Persistence.Configurations;

public class ForgotPasswordConfig : IEntityTypeConfiguration<ForgotPassword>
{
    public void Configure(EntityTypeBuilder<ForgotPassword> builder)
    {
        builder.ToTable("ForgotPassword");

        builder.HasKey(x => x.Id);

        //builder.Property(k => k.Id).UseIdentityColumn();

        builder.Property(x => x.Email);

    }
}
