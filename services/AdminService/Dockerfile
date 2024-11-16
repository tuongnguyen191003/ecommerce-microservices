# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy Shared Library
COPY ./shared/SharedLibrary ./shared/SharedLibrary
RUN dotnet build ./shared/SharedLibrary/SharedLibrary.csproj

# Copy AdminService csproj and restore dependencies
COPY ./services/AdminService/AdminService.csproj ./services/AdminService/
WORKDIR /app/services/AdminService
RUN dotnet restore

# Copy the rest of the AdminService code
COPY ./services/AdminService ./services/AdminService
RUN dotnet publish AdminService.csproj -c Release -o /app/out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the application
ENTRYPOINT ["dotnet", "AdminService.dll"]
