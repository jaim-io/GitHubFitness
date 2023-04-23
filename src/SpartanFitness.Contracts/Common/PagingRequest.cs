namespace SpartanFitness.Contracts.Common;

public record PagingRequest(
  int? Page,
  int? Size,
  string? Sort,
  string? Search);