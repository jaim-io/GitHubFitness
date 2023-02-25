namespace SpartanFitness.Application.Authentication.Queries;

public record LoginQuery(
    string Email,
    string Password);