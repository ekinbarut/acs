FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Albelli.API/Albelli.API.csproj", "Albelli.API/"]
RUN dotnet restore "Albelli.API/Albelli.API.csproj"
COPY . .
WORKDIR "/src/Albelli.API"
RUN dotnet build "Albelli.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Albelli.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Albelli.API.dll"]
