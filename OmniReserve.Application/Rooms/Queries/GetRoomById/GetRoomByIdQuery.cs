using MediatR;

namespace OmniReserve.Application.Rooms.Queries.GetRoomById;

public record GetRoomByIdQuery(Guid RoomId) : IRequest<RoomResponseDto>;