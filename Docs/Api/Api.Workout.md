# Spartan Fitness Workout API

- [Spartan Fitness Workout API](#spartan-fitness-workout-api)
  - [Workout](#workout)
    - [Create Workout Request](#create-workout-request)
    - [Create Workout Response](#create-workout-response)

## Workout

___

### Create Workout Request

```js
POST {{host}}/api/v1/coaches/{{coachId}}/workout
```

```json
{
    "name": "Push",
    "description": "Lorem Ipsum",
    "workoutImage": "https://randomimage.com",
    "image": "https://randomimage.com",
    "muscleGroupIds": [
       "00000000-0000-0000-0000-000000000000",
    ],
       "workoutExercises": [
        { 
            "orderNumber": 1,
            "exerciseId": "00000000-0000-0000-0000-000000000000" ,
            "sets": 3,
            "repRange": {
                "minReps": 8,
                "maxReps": 10
            },
            "type": "dropset", // default, dropset, superset, ...
        },
    ],
}
```

### Create Workout Response

```js
201 CREATED
```

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "name": "Push",
    "description": "Lorem Ipsum",
    "coachId": "00000000-0000-0000-0000-000000000000",
    "image": "https://randomimage.com",
    "muscleGroupIds": [
        "00000000-0000-0000-0000-000000000000",
    ],
    "workoutExercises": [
        { 
            "id": "00000000-0000-0000-0000-000000000000",
            "orderNumber": 1,
            "exerciseId": "00000000-0000-0000-0000-000000000000",
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