# Build

FROM mcr.microsoft.com/dotnet/sdk:7.0.203 AS build

ENV ASPNETCORE_ENVIRONMENT=Production

WORKDIR /source

COPY . .

RUN dotnet restore "./RedditMockup.Web/RedditMockup.Web.csproj" --disable-parallel

RUN dotnet publish "./RedditMockup.Web/RedditMockup.Web.csproj" -o /app --no-restore

# Serve

FROM mcr.microsoft.com/dotnet/aspnet:7.0.5

WORKDIR /app

COPY --from=build /app .

ENTRYPOINT ["dotnet", "RedditMockup.Web.dll"]