# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copy Shared Library
COPY ./shared/SharedLibrary ./shared/SharedLibrary
RUN dotnet build ./shared/SharedLibrary/SharedLibrary.csproj

# Copy ProductService csproj and restore dependencies
COPY ./services/ProductService/ProductService.csproj ./
RUN dotnet restore

# Copy the rest of the ProductService code
COPY ./services/ProductService ./
RUN dotnet publish ProductService.csproj -c Release -o out

# Stage 2: Build a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Run the application
ENTRYPOINT ["dotnet", "ProductService.dll"]
