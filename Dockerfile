FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WalletSystem.Services/WalletSystem.Services.API/WalletSystem.Services.API.csproj", "WalletSystem.Services/WalletSystem.Services.API/"]
RUN dotnet restore "WalletSystem.Services/WalletSystem.Services.API/WalletSystem.Services.API.csproj"
COPY . .
WORKDIR "/src/WalletSystem.Services/WalletSystem.Services.API"
RUN dotnet build "WalletSystem.Services.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WalletSystem.Services.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WalletSystem.Services.API.dll"]
