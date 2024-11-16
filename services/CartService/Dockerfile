# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy Shared Library
COPY ./shared/SharedLibrary ./shared/SharedLibrary
RUN dotnet build ./shared/SharedLibrary/SharedLibrary.csproj

# Copy CartService csproj and restore dependencies
COPY ./services/CartService/CartService.csproj ./services/CartService/
WORKDIR /app/services/CartService
RUN dotnet restore

# Copy the rest of the CartService code
COPY ./services/CartService ./services/CartService
RUN dotnet publish CartService.csproj -c Release -o /app/out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the application
ENTRYPOINT ["dotnet", "CartService.dll"]
