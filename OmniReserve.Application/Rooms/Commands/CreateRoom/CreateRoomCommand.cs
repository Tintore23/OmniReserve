using MediatR;
using OmniReserve.Domain.Entities;


namespace OmniReserve.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommand : IRequest<Guid>
{
    public string RoomNumber { get; set; } = string.Empty;

    public RoomType Type { get; set; }

    public decimal PricePerNight { get; set; }
}
