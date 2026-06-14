FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BmWms.Web/BmWms.Web.csproj", "BmWms.Web/"]
COPY ["BmWms.Infrastructure/BmWms.Infrastructure.csproj", "BmWms.Infrastructure/"]
COPY ["BmWms.Core/BmWms.Core.csproj", "BmWms.Core/"]
RUN dotnet restore "./BmWms.Web/BmWms.Web.csproj"
COPY . .
WORKDIR "/src/BmWms.Web"
RUN dotnet build "./BmWms.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BmWms.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BmWms.Web.dll"]