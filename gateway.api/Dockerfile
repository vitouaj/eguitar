FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5226

ENV ASPNETCORE_URLS=http://+:5226

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["gateway.api/gateway.api.csproj", "gateway.api/"]
RUN dotnet restore "gateway.api/gateway.api.csproj"
COPY . .
WORKDIR "/src/gateway.api"
RUN dotnet build "gateway.api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "gateway.api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "gateway.api.dll"]
