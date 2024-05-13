FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LOLA-SERVICE.API/LOLA-SERVICE.API.csproj", "LOLA-SERVICE.API/"]
RUN dotnet restore "LOLA-SERVICE.API/LOLA-SERVICE.API.csproj"
COPY . .
WORKDIR "/src/LOLA-SERVICE.API"
RUN dotnet build "LOLA-SERVICE.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LOLA-SERVICE.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LOLA-SERVICE.API.dll"]
