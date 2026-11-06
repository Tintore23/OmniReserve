using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using OmniReserve.Application.Rooms.Queries.GetRoomById; // Ajusta según la ruta real de tu DTO
using OmniReserve.Application.Common.Interfaces; // O la ruta donde tengas tus interfaces               // Asegúrate de tener este namespace para IRoomRepository

namespace OmniReserve.Application.Rooms.Queries.GetAllRooms;

public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, List<RoomResponseDto>>
{
    private readonly IDistributedCache _cache;
    private readonly IRoomRepository _roomRepository;
    private const string CACHE_KEY = "rooms_catalog";

    public GetAllRoomsQueryHandler(IDistributedCache cache, IRoomRepository roomRepository)
    {
        _cache = cache;
        _roomRepository = roomRepository;
    }

    public async Task<List<RoomResponseDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
    {
        // Paso A: Intentar obtener las habitaciones desde la caché de Redis
        var cachedRooms = await _cache.GetStringAsync(CACHE_KEY, cancellationToken);

        // Paso B: Si existen datos en caché, deserializar y retornar inmediatamente
        if (!string.IsNullOrEmpty(cachedRooms))
        {
            var roomsFromCache = JsonSerializer.Deserialize<List<RoomResponseDto>>(cachedRooms);
            if (roomsFromCache is not null)
            {
                return roomsFromCache;
            }
        }

        // Paso C: Si la caché está vacía, consultar la base de datos a través del repositorio
        var roomsFromDb = await _roomRepository.GetAllAsync(cancellationToken);

        // Mapeo de entidades a DTOs usando las propiedades reales de Room.cs
        var roomDtos = roomsFromDb.Select(r => new RoomResponseDto(
            r.Id,
            r.RoomNumber,
            r.Type.ToString(), 
            r.PricePerNight,
            r.IsAvailable
        )).ToList();

        // Paso D: Serializar la lista obtenida a formato JSON
        var serializedRooms = JsonSerializer.Serialize(roomDtos);

        // Paso E: Configurar la expiración del caché (ej. 5 minutos)
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };

        // Guardar el resultado en Redis
        await _cache.SetStringAsync(CACHE_KEY, serializedRooms, cacheOptions, cancellationToken);

        // Retornar la lista de habitaciones
        return roomDtos;
    }
}
//sin fix aun no se que son los dockers