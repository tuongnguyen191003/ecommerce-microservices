# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy Shared Library
COPY ./shared/SharedLibrary ./shared/SharedLibrary
RUN dotnet build ./shared/SharedLibrary/SharedLibrary.csproj

# Copy OrderService csproj and restore dependencies
COPY ./services/OrderService/OrderService.csproj ./services/OrderService/
WORKDIR /app/services/OrderService
RUN dotnet restore

# Copy the rest of the OrderService code
COPY ./services/OrderService ./services/OrderService
RUN dotnet publish OrderService.csproj -c Release -o /app/out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the application
ENTRYPOINT ["dotnet", "OrderService.dll"]
