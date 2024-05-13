FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LOLA-SERVICE.Api/LOLA-SERVICE.Api.csproj", "LOLA-SERVICE.Api/"]
RUN dotnet restore "LOLA-SERVICE.Api/LOLA-SERVICE.Api.csproj"
COPY . .
WORKDIR "/src/LOLA-SERVICE.Api"
RUN dotnet build "LOLA-SERVICE.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LOLA-SERVICE.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LOLA-SERVICE.Api.dll"]
