# Build

FROM mcr.microsoft.com/dotnet/sdk:7.0.203 AS build

ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /source

COPY . .

RUN dotnet restore ./DataSelector.Web/DataSelector.Web.csproj --disable-parallel

RUN dotnet publish ./DataSelector.Web/DataSelector.Web.csproj -o /publish --no-restore

# Serve

FROM mcr.microsoft.com/dotnet/aspnet:7.0.5

WORKDIR /app

COPY --from=build /publish ./

ENTRYPOINT ["dotnet", "DataSelector.Web.dll"]