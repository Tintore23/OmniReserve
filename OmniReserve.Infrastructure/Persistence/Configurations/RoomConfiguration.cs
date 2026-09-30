using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OmniReserve.Domain;
using OmniReserve.Domain.Entities;

namespace OmniReserve.Infrastructure.Persistence.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
       builder.ToTable("Rooms");

        builder.HasKey(room => room.Id);

        builder.Property(room => room.RoomNumber)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(room => room.PricePerNight)
            .HasPrecision(18, 2);

        builder.Property(room => room.Type)
            .HasConversion<string>();
    }
}