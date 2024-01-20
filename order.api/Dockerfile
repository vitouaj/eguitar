FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5225

ENV ASPNETCORE_URLS=http://+:5225

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["order.api/order.api.csproj", "order.api/"]
RUN dotnet restore "order.api/order.api.csproj"
COPY . .
WORKDIR "/src/order.api"
RUN dotnet build "order.api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "order.api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "order.api.dll"]
