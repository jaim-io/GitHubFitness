using ErrorOr;

using MediatR;

namespace SpartanFitness.Application.Users.Commands.UnSaveMuscleRange;

public record UnSaveMuscleRangeCommand(string UserId, List<string> MuscleIds) : IRequest<ErrorOr<Unit>>;