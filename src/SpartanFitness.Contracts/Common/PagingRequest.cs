using Microsoft.AspNetCore.Mvc;

namespace SpartanFitness.Contracts.Common;

public record PagingRequest
{
  [FromQuery(Name = "p")]
  public int? Page { get; init; }

  [FromQuery(Name = "ls")]
  public int? Size { get; init; }

  [FromQuery(Name = "s")]
  public string? Sort { get; init; }

  [FromQuery(Name = "o")]
  public string? Order { get; init; }

  [FromQuery(Name = "q")]
  public string? Query { get; init; }
}