﻿using SpartanFitness.Domain.Aggregates;
using SpartanFitness.Domain.ValueObjects;

namespace SpartanFitness.Application.Common.Interfaces.Persistence;

public interface IMuscleRepository
{
  Task AddAsync(Muscle muscle);
  Task<IEnumerable<Muscle>> GetAllAsync();
  Task<Muscle?> GetByIdAsync(MuscleId id);
  Task<Muscle?> GetByNameAsync(string name);
}