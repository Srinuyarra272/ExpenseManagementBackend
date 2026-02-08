# Multi-stage Dockerfile for .NET 8 ASP.NET Core API

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["ExpenseTracker.API/ExpenseTracker.API.csproj", "ExpenseTracker.API/"]
RUN dotnet restore "ExpenseTracker.API/ExpenseTracker.API.csproj"

# Copy everything and publish
COPY . .
WORKDIR /src/ExpenseTracker.API
RUN dotnet publish "ExpenseTracker.API.csproj" -c Release -o /app/publish /p:TrimUnusedDependencies=true

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
COPY --from=build /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "ExpenseTracker.API.dll"]
