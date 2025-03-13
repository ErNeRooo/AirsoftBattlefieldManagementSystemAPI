
# build
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build

COPY . /source

WORKDIR /source

ARG TARGETARCH

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

RUN dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app


# final
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS final
WORKDIR /app

COPY --from=build /app .

USER $APP_UID

ENTRYPOINT ["dotnet", "AirsoftBattlefieldManagementSystemAPI.dll"]


# development
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS dev
WORKDIR /app

COPY ["./AirsoftBattlefieldManagementSystemAPI.sln", "./"]
COPY ["src/AirsoftBattlefieldManagementSystemAPI.csproj", "./src/"]
COPY ["tests/AirsoftBattlefieldManagementSystemAPI.Tests/AirsoftBattlefieldManagementSystemAPI.Tests.csproj", "tests/AirsoftBattlefieldManagementSystemAPI.Tests/"]

RUN dotnet nuget locals all --clear && dotnet restore "AirsoftBattlefieldManagementSystemAPI.sln"

COPY . .

ENV DOTNET_USE_POLLING_FILE_WATCHER 1
ENTRYPOINT ["dotnet", "watch", "run", "--no-launch-profile", "--urls=http://0.0.0.0:8080", "--project", "src/AirsoftBattlefieldManagementSystemAPI.csproj"]