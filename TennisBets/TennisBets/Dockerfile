# Production runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TennisBets/TennisBets/TennisBets.csproj", "TennisBets/TennisBets/"]
RUN dotnet restore "TennisBets/TennisBets/TennisBets.csproj"
COPY . .
WORKDIR "/src/TennisBets/TennisBets"
RUN dotnet build "TennisBets.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TennisBets.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TennisBets.dll"]