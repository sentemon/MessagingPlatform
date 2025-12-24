FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 9000

RUN apt-get update && apt-get install -y curl && apt-get clean

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY MessagingPlatform.Api/MessagingPlatform.Api.csproj MessagingPlatform.Api/
COPY MessagingPlatform.Application/MessagingPlatform.Application.csproj MessagingPlatform.Application/
COPY MessagingPlatform.Domain/MessagingPlatform.Domain.csproj MessagingPlatform.Domain/
COPY MessagingPlatform.Infrastructure/MessagingPlatform.Infrastructure.csproj MessagingPlatform.Infrastructure/

RUN dotnet restore MessagingPlatform.Api/MessagingPlatform.Api.csproj

COPY . .

WORKDIR /src/MessagingPlatform.Api

RUN dotnet build MessagingPlatform.Api.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish MessagingPlatform.Api.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MessagingPlatform.Api.dll"]
