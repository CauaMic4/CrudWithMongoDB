This is a REST API project built with .NET 8 and MongoDB, focused on creating an authentication and user management system.

What was done in this project?
.NET 8 REST API: All endpoints for login and user CRUD.

MongoDB Integration: Uses the MongoDB.Driver for NoSQL data persistence.

Layered Architecture (DDD): The project is separated into 4 main layers:

Domain: Contains entities (e.g., User) and repository interfaces.

Application: Contains business logic (Services/Business), VOs (DTOs), and converters.

Infrastructure: Implements repositories and infra-level services (e.g., TokenService, MongoDbContext).

Api: The presentation layer (Controllers) that exposes the endpoints.

JWT (JSON Web Token) Authentication:

Login endpoint (/api/auth/signin) that validates the user and returns an Access Token and Refresh Token.

Refresh endpoint (/api/auth/refresh) that generates a new token from a valid refresh token.

Revoke endpoint (/api/auth/revoke) to invalidate a user's refresh token (logout).

Full User CRUD:

POST /api/user: Creates a new user.

GET /api/user/{id}: Finds a user by ID.

GET /api/user: Lists all users.

PUT /api/user: Updates a user.

DELETE /api/user/{id}: Deactivates a user (soft delete).

Security:

Password Hashing: Passwords are hashed using SHA256 before being stored in the database.

Protected Routes: The User CRUD operations are protected by [Authorize("Bearer")].

How to Run
Clone the repository.

Configure your appsettings.json with the MongoDbSettings (your connection string) and TokenConfiguration (your Secret, Issuer, etc.) sections.

Run the ProjectMongo.API project.

--------------------------------------------------------------------------------------------------------------------------------

Este é um projeto de API REST construído com .NET 8 e MongoDB, focado em criar um sistema de autenticação e gerenciamento de usuários.

O que foi feito neste projeto?
API REST com .NET 8: Todos os endpoints para login e CRUD de usuários.

Integração com MongoDB: Uso do MongoDB.Driver para persistência de dados NoSQL.

Arquitetura em Camadas (DDD): O projeto é separado em 4 camadas principais:

Domain: Contém as entidades (ex: User) e as interfaces dos repositórios.

Application: Contém a lógica de negócio (Services/Business), VOs (DTOs) e conversores.

Infrastructure: Implementa os repositórios e serviços de infra (ex: TokenService, MongoDbContext).

Api: A camada de apresentação (Controllers) que expõe os endpoints.

Autenticação JWT (JSON Web Token):

Endpoint de Login (/api/auth/signin) que valida o usuário e retorna um Access Token e um Refresh Token.

Endpoint de Refresh (/api/auth/refresh) que gera um novo token a partir de um refresh token válido.

Endpoint de Revoke (/api/auth/revoke) para invalidar o refresh token de um usuário (logout).

CRUD Completo de Usuários:

POST /api/user: Cria um novo usuário.

GET /api/user/{id}: Busca um usuário por ID.

GET /api/user: Lista todos os usuários.

PUT /api/user: Atualiza um usuário.

DELETE /api/user/{id}: Desativa um usuário (soft delete).

Segurança:

Hashing de Senhas: As senhas são tratadas com SHA256 antes de serem salvas no banco.

Rotas Protegidas: O CRUD de usuários é protegido por [Authorize("Bearer")].

Como Executar
Clone o repositório.

Configure seu appsettings.json com as seções MongoDbSettings (sua connection string) e TokenConfiguration (seu Secret, Issuer, etc.).

Execute o projeto ProjectMongo.API.
