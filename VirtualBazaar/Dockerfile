FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VirtualBazaar/VirtualBazaar.csproj", "VirtualBazaar/"]
RUN dotnet restore "VirtualBazaar/VirtualBazaar.csproj"
COPY . .
WORKDIR "/src/VirtualBazaar"
RUN dotnet build "VirtualBazaar.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "VirtualBazaar.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VirtualBazaar.dll"]
