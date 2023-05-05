namespace SpartanFitness.Domain.Common.Models;

public record Pagination<T>(
  List<T> Content,
  int PageNumber,
  int PageCount);