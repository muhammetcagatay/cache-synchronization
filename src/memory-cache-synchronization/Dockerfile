#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["memory-cache-synchronization.csproj", "."]
RUN dotnet restore "./memory-cache-synchronization.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "memory-cache-synchronization.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "memory-cache-synchronization.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "memory-cache-synchronization.dll"]