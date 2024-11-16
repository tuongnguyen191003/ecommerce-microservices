# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy Shared Library
COPY ./shared/SharedLibrary ./shared/SharedLibrary
RUN dotnet build ./shared/SharedLibrary/SharedLibrary.csproj

# Copy NotificationService csproj and restore dependencies
COPY ./services/NotificationService/NotificationService.csproj ./services/NotificationService/
WORKDIR /app/services/NotificationService
RUN dotnet restore

# Copy the rest of the NotificationService code
COPY ./services/NotificationService ./services/NotificationService
RUN dotnet publish NotificationService.csproj -c Release -o /app/out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the application
ENTRYPOINT ["dotnet", "NotificationService.dll"]
