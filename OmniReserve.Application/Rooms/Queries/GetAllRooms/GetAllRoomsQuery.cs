using MediatR;
using OmniReserve.Application.Rooms.Queries.GetRoomById; // Ajusta según la ubicación de tu DTO

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public record GetAllRoomsQuery() : IRequest<List<RoomResponseDto>>;