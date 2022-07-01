using JobService.SqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobService.SqlServer.Configuration;

public class JobEntityTypeConfiguration : IEntityTypeConfiguration<JobEntity>
{
    public void Configure(EntityTypeBuilder<JobEntity> builder)
    {
        builder.Property(entity => entity.JobDto)
            .HasConversion(Converters.ObjectAsJsonValueConverter);

        builder.Property(entity => entity.JobStatus)
            .HasConversion(Converters.EnumValueConverter<JobStatus>());

        builder.HasIndex(entity => entity.JobStatus);
    }
}