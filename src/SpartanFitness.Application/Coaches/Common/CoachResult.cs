using SpartanFitness.Domain.Aggregates;

namespace SpartanFitness.Application.Coaches.Common;

public record CoachResult(
  User User,
  Coach Coach);