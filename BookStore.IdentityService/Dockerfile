#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 6004
EXPOSE 6005

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BookStore.IdentityService/BookStore.IdentityService.csproj", "BookStore.IdentityService/"]
COPY ["BookStore.Core/BookStore.Core.csproj", "BookStore.Core/"]
RUN dotnet restore "./BookStore.IdentityService/./BookStore.IdentityService.csproj"
COPY . .
WORKDIR "/src/BookStore.IdentityService"
RUN dotnet build "./BookStore.IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BookStore.IdentityService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookStore.IdentityService.dll"]