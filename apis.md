# Authentication API Documentation

## Endpoints

### Register User
Creates a new user account.

**Endpoint:** `POST /api/Auth/Register`

**Request Body:**
```json
{
  "username": "string",
  "email": "string",
  "password": "string"
}
```

**Responses:**

- **200 OK**
```json
{
  "userId": 0,
  "username": "string",
  "email": "string",
  "password": "string",
  "ownedLobbies": [],
  "lobbyPlayers": [],
  "ratingsGiven": [],
  "ratingsReceived": [],
  "sentMessages": []
}
```

- **400 Bad Request**
```json
{
  "message": "Email already in use."
}
```

### Login User
Authenticates a user and returns their information.

**Endpoint:** `POST /api/Auth/Login`

**Request Body:**
```json
{
  "email": "string",
  "password": "string"
}
```

**Responses:**

- **200 OK**
```json
{
  "userId": 0,
  "username": "string",
  "email": "string",
  "password": "string",
  "ownedLobbies": [],
  "lobbyPlayers": [],
  "ratingsGiven": [],
  "ratingsReceived": [],
  "sentMessages": []
}
```

- **400 Bad Request**
```json
{
  "message": "Invalid email."
}
```
OR
```json
{
  "message": "Invalid password."
}
```
