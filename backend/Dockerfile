FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 9000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["MessagingPlatform.sln", "./"]
COPY ["src/MessagingPlatform.Api/MessagingPlatform.Api.csproj", "src/MessagingPlatform.Api/"]
COPY ["src/MessagingPlatform.Application/MessagingPlatform.Application.csproj", "src/MessagingPlatform.Application/"]
COPY ["src/MessagingPlatform.Domain/MessagingPlatform.Domain.csproj", "src/MessagingPlatform.Domain/"]
COPY ["src/MessagingPlatform.Infrastructure/MessagingPlatform.Infrastructure.csproj", "src/MessagingPlatform.Infrastructure/"]

RUN dotnet restore "src/MessagingPlatform.Api/MessagingPlatform.Api.csproj"

COPY . .

WORKDIR "/src/src/MessagingPlatform.Api"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MessagingPlatform.Api.dll"]
