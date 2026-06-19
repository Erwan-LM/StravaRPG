FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["StravaRpg.Api.csproj", "./"]
RUN dotnet restore "StravaRpg.Api.csproj"
COPY . .
RUN dotnet publish "StravaRpg.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM runtime AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "StravaRpg.Api.dll"]
