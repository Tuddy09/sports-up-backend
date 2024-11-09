---

# AuthController API Documentation

## Base URL

```
/api/Auth
```

### **Endpoints**

1. [Register User](#register-user)
2. [Login User](#login-user)

---

## Register User

**Endpoint:**  
`POST /api/Auth/Register`

**Description:**  
Registers a new user by adding their details to the database.

**Request Body:**

```json
{
  "UserId": int,
  "Username": "string",
  "Email": "string",
  "Password": "string"
}
```

**Response:**

- **201 Created**  
  Success response with the created user object.

  ```json
  {
    "userId": 1,
    "username": "JohnDoe",
    "email": "johndoe@example.com",
    "ownedLobbies": [],
    "lobbyPlayers": [],
    "ratingsGiven": [],
    "ratingsReceived": [],
    "sentMessages": []
  }
  ```

- **400 Bad Request**  
  Error response if the email is already in use.

  ```json
  {
    "error": "Email already in use."
  }
  ```

---

## Login User

**Endpoint:**  
`POST /api/Auth/Login`

**Description:**  
Authenticates a user by checking their email and password.

**Request Body:**

```json
{
  "Email": "string",
  "Password": "string"
}
```

**Response:**

- **200 OK**  
  Success response with the authenticated user object.

  ```json
  {
    "userId": 1,
    "username": "JohnDoe",
    "email": "johndoe@example.com",
    "ownedLobbies": [],
    "lobbyPlayers": [],
    "ratingsGiven": [],
    "ratingsReceived": [],
    "sentMessages": []
  }
  ```

- **400 Bad Request**  
  Error response for invalid email or password.

  ```json
  {
    "error": "Invalid email."
  }
  ```

  ```json
  {
    "error": "Invalid password."
  }
  ```

---

**Models**

### User

| Field          | Type   | Description                   |
|----------------|--------|-------------------------------|
| `UserId`       | int    | Unique identifier for the user|
| `Username`     | string | Username of the user          |
| `Email`        | string | User's email address          |
| `Password`     | string | User's password               |
| `OwnedLobbies` | array  | List of lobbies owned by user |
| `LobbyPlayers` | array  | List of lobbies user has joined|
| `RatingsGiven` | array  | List of ratings given by user |
| `RatingsReceived` | array | List of ratings received by user |
| `SentMessages` | array  | List of messages sent by user |
