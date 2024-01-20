FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5224

ENV ASPNETCORE_URLS=http://+:5224

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["product.api/product.api.csproj", "product.api/"]
RUN dotnet restore "product.api/product.api.csproj"
COPY . .
WORKDIR "/src/product.api"
RUN dotnet build "product.api.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "product.api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "product.api.dll"]
