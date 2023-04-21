# Spartan Fitness Auth API

- [Spartan Fitness Auth API](#spartan-fitness-auth-api)
    - [Register](#register)
      - [Register Request](#register-request)
      - [Register Response](#register-response)
    - [Login](#login)
      - [Login Request](#login-request)
      - [Login Response](#login-response)

<br/>

### Register

___

#### Register Request

```js
POST {{host}}/auth/register
```

```json
{
    "firstName": "Jamey",
    "lastName": "Schaap",
    "profileImage": "https://randomimage.com",
    "email": "jameyschaap@githubfitness.com",
    "password": "jamey123!"
}
```

#### Register Response

```js
200 OK
```

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "firstName": "Jamey",
    "lastName": "Schaap",
    "profileImage": "https://randomimage.com",
    "email": "jameyschaap@githubfitness.com",
    "token": "eyJbh..z0dqcnXoY"
}
```

<br/><br/>

### Login

___

#### Login Request

```js
POST {{host}}/auth/register
```

```json
{
    "email": "jameyschaap@githubfitness.com",
    "password": "jamey123!"
}
```

#### Login Response

```js
200 OK
```

```json
{
    "id": "00000000-0000-0000-0000-000000000000",
    "firstName": "Jamey",
    "lastName": "Schaap",
    "profileImage": "https://randomimage.com",
    "email": "jameyschaap@githubfitness.com",
    "token": "eyJbh..z0dqcnXoY"
}
```
