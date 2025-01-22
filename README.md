
# ModeforcesAPI

## Overview

ModeforcesAPI is a backend API built with C# that handles functionalities similar to Codeforces, including managing problems and submissions. This README provides detailed instructions on how to set up, run, and contribute to the project.

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.
- [Docker](https://www.docker.com/get-started) installed on your machine (optional, for Docker setup).

## Getting Started

### Cloning the Repository

First, clone the repository to your local machine:

```bash
git clone https://github.com/jasserhouimli/modeforcesAPI.git
cd modeforcesAPI
```

### Building the Project

To build the project, run the following command in the root directory of the repository:

```bash
dotnet build
```

### Running the Project

To run the project locally, use the following command:

```bash
dotnet run
```

The application should now be running on `http://localhost:5000`.

## Docker Setup (Optional)

To build and run the project using Docker, follow these steps:

### Building the Docker Image

```bash
docker build -t modeforcesapi .
```

### Running the Docker Container

```bash
docker run -d -p 8080:80 --name modeforcesapi_container modeforcesapi
```

The application should now be accessible at `http://localhost:8080`.

## Configuration

Configuration settings can be found in the `appsettings.json` and `appsettings.Development.json` files. Adjust these files as needed to configure the application.

### Example Configuration (`appsettings.json`)

```json
{
  \"Logging\": {
    \"LogLevel\": {
      \"Default\": \"Information\",
      \"Microsoft.AspNetCore\": \"Warning\"
    }
  },
  \"AllowedHosts\": \"*\",
  \"Kestrel\": {
    \"Endpoints\": {
      \"Http1\": {
        \"Url\": \"http://0.0.0.0:7100\"
      }
    }
  },
  \"Jwt\": {
    \"SecretKey\": \"ThisIsASecretKeyThatIsAtLeast128BitsLong!!\",
    \"Issuer\": \"Swag\",
    \"Audience\": \"modeforces\"
  }
}
```

## Project Structure

- **Controller**: Contains controllers for handling HTTP requests.
- **Data**: Contains data access layer and context classes.
- **Languages**: Contains language-related functionality (purpose needs to be further clarified).
- **Migrations**: Contains migration files for database schema changes.
- **Models**: Contains the data models used in the application.
- **Properties**: Contains project properties and settings.
- **Program.cs**: Entry point of the application.
- **zerops.yml**: Configuration file for Zerops deployment.

## API Endpoints

The project includes an example HTTP request file (`api.http`) for testing the endpoints:

```http
@api_HostAddress = http://localhost:5236

GET {{api_HostAddress}}/weatherforecast/
Accept: application/json
```

## Database

The project uses a PostgreSQL database. The connection string is configured in the `Program.cs` file:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(\"Host=db;Database=db;Username=db;Password=zxZdA8BYRGgDbym8\");
});
```

## Security

The project uses JWT for authentication. The JWT configuration can be found in the `Program.cs` and `appsettings.json` files:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration[\"Jwt:Issuer\"],
            ValidAudience = builder.Configuration[\"Jwt:Audience\"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration[\"Jwt:SecretKey\"]))
        };
    });
```

## Contributing

If you want to contribute to the project, please follow these steps:
1. Fork the repository.
2. Create a new branch (`git checkout -b feature-branch`).
3. Make your changes.
4. Commit your changes (`git commit -am 'Add some feature'`).
5. Push to the branch (`git push origin feature-branch`).
6. Create a new Pull Request.

## License

ModeforcesAPI is licensed under the [MIT License](LICENSE).

---



