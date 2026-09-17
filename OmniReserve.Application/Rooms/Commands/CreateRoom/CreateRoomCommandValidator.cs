using FluentValidation;

namespace OmniReserve.Application.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.RoomNumber)
            .NotEmpty()
            .MaximumLength(5);

        RuleFor(x => x.PricePerNight)
            .GreaterThan(0);
    }
}
