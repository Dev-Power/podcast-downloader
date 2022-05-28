FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PodcastDownloader.WorkerService/PodcastDownloader.WorkerService.csproj", "PodcastDownloader.WorkerService/"]
RUN dotnet restore "PodcastDownloader.WorkerService/PodcastDownloader.WorkerService.csproj"
COPY . .
WORKDIR "/src/PodcastDownloader.WorkerService"
RUN dotnet build "PodcastDownloader.WorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PodcastDownloader.WorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PodcastDownloader.WorkerService.dll"]
