FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["IPTV.WebApi/IPTV.WebApi.csproj", "IPTV.WebApi/"]
RUN dotnet restore "IPTV.WebApi/IPTV.WebApi.csproj"
COPY . .
WORKDIR "/src/IPTV.WebApi"
RUN dotnet build "IPTV.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IPTV.WebApi.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IPTV.WebApi.dll"]
