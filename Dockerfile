# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy solution and csproj files
COPY OrderManagementSystem/*.sln ./OrderManagementSystem/
COPY OrderManagementSystem/*.csproj ./OrderManagementSystem/

# Restore dependencies
RUN dotnet restore

# Copy all source files
COPY . .

# Publish the project
RUN dotnet publish OrderManagementSystem/OrderManagementSystem.csproj -c Release -o /app/out

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/out .

# Start the application
ENTRYPOINT ["dotnet", "OrderManagementSystem.dll"]
