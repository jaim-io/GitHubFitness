# Domain Aggregates

## Workout

```cs
class Workout
{
    // TODO: Add methods
}
```

```json
{
    "id": { "value": "00000000-0000-0000-0000-000000000000" },
    "name": "Push",
    "description": "Lorem Ipsum",
    "coachId": { "value": "00000000-0000-0000-0000-000000000000" },
    "image": "https://randomimage.com",
    "muscleGroupIds": [
        { "value": "00000000-0000-0000-0000-000000000000" },
    ],
    "workoutExercises": [
        { 
            "id": { "value": "00000000-0000-0000-0000-000000000000" },
            "orderNumber": 1,
            "exerciseId": { "value": "00000000-0000-0000-0000-000000000000" },
            "sets": 3,
            "repRange": {
                "minReps": 8,
                "maxReps": 10
            },
            "type": "dropset", // default, dropset, superset, ...
        },
    ],
    "createdDateTime": "2023-01-01T00:00:00.0000000Z",
    "updatedDateTime": "2023-01-01T00:00:00.0000000Z"
}
```
