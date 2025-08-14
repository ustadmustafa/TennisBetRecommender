# TennisBetRecommender

Tennis betting recommendation system built with .NET Core 8.0

## 🚀 Quick Start with Docker

### Prerequisites
- Docker Desktop
- Docker Compose

### Run with Docker
```bash
# Start the application
docker-compose up --build

# Or use PowerShell scripts (Windows)
.\docker-start.ps1
```

### Access the Application
- HTTP: http://localhost:8080
- HTTPS: https://localhost:8081

### Stop the Application
```bash
docker-compose down

# Or use PowerShell script (Windows)
.\docker-stop.ps1
```

## 🛠️ Development

### Traditional Development
```bash
cd TennisBets
dotnet restore
dotnet run
```

### Docker Development
```bash
# Development mode with hot reload
docker-compose up --build

# Production mode
docker-compose -f docker-compose.prod.yml up --build
```

## 📁 Project Structure

- `TennisBets/` - Main application
- `docker-compose.yml` - Development Docker configuration
- `docker-compose.prod.yml` - Production Docker configuration
- `DOCKER_README.md` - Detailed Docker documentation
- `docker-start.ps1` - Windows PowerShell start script
- `docker-stop.ps1` - Windows PowerShell stop script

## 🔧 Docker Commands

```bash
# Build image
docker build -t tennisbets -f TennisBets/Dockerfile .

# Run container
docker run -p 8080:8080 -p 8081:8081 tennisbets

# View logs
docker-compose logs -f tennisbets

# Clean up
docker-compose down --rmi all --volumes
```

## 📚 More Information

For detailed Docker usage instructions, see [DOCKER_README.md](DOCKER_README.md).