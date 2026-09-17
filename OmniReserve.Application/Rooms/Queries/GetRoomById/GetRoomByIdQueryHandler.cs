using MediatR;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomResponseDto>
{
    public Task<RoomResponseDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        // Simulación de respuesta recuperada desde la base de datos
        var response = new RoomResponseDto(
            Id: request.RoomId,
            RoomNumber: "101-A",
            Type: "Suite",
            Price: 150.00m,
            IsAvailable: true
        );

        return Task.FromResult(response);
    }
}