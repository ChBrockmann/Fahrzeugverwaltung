FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Fahrzeugverwaltung.Microservice.Benachrichtigung.Service/Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.csproj", "Fahrzeugverwaltung.Microservice.Benachrichtigung.Service/"]
RUN dotnet restore "Fahrzeugverwaltung.Microservice.Benachrichtigung.Service/Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.csproj"
COPY . .
WORKDIR "/src/Fahrzeugverwaltung.Microservice.Benachrichtigung.Service"
RUN dotnet build "Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.dll"]
