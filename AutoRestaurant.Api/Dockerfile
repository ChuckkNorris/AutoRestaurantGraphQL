FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AutoRestaurant.Api/AutoRestaurant.Api.csproj", "AutoRestaurant.Api/"]
RUN dotnet restore "AutoRestaurant.Api/AutoRestaurant.Api.csproj"
COPY . .
WORKDIR "/src/AutoRestaurant.Api"
RUN dotnet build "AutoRestaurant.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AutoRestaurant.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AutoRestaurant.Api.dll"]