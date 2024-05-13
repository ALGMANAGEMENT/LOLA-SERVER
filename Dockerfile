FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LOLA-SERVER.Api/LOLA-SERVER.Api.csproj", "LOLA-SERVER.Api/"]
RUN dotnet restore "LOLA-SERVER.Api/LOLA-SERVER.Api.csproj"
COPY . .
WORKDIR "/src/LOLA-SERVER.Api"
RUN dotnet build "LOLA-SERVER.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LOLA-SERVER.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LOLA-SERVER.Api.dll"]
