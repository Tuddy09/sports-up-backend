# Authentication API Documentation

## Overview
The Authentication API provides endpoints for user registration and login functionality. This API is part of the Sports Up Backend service.

## Base URL
```
/api/Auth
```

## Models

### User
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

### LoginRequest
```json
{
  "email": "string",
  "password": "string"
}
```

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

## Security Considerations

1. The API currently returns the password field in responses, which is not recommended for production environments.
2. Passwords are currently stored and compared in plain text. It's recommended to implement password hashing.
3. Consider implementing JWT or another token-based authentication system for maintaining user sessions.

## Data Relationships

The User model maintains several relationships:
- OwnedLobbies: Lobbies created by the user
- LobbyPlayers: Lobbies where the user is a participant
- RatingsGiven: Ratings provided to other players
- RatingsReceived: Ratings received from other players
- SentMessages: Messages sent by the user

## Error Handling

The API returns appropriate HTTP status codes:
- 200: Successful operation
- 400: Bad request (invalid inputs or business rule violations)
