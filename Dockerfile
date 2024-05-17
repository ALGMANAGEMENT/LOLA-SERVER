FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["LOLA-SERVER.API/LOLA-SERVER.API.csproj", "LOLA-SERVER.API/"]
RUN dotnet restore "LOLA-SERVER.API/LOLA-SERVER.API.csproj"
COPY . .
WORKDIR "/src/LOLA-SERVER.API"
RUN dotnet build "LOLA-SERVER.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LOLA-SERVER.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LOLA-SERVER.API.dll"]
