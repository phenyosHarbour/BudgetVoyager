# Use a Windows-based image with .NET Runtime
FROM mcr.microsoft.com/dotnet/framework/runtime:4.8-windowsservercore-ltsc2019 AS base
WORKDIR /app

# Use an image with .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build
WORKDIR /src
COPY ["Budget Tracker App/Budget Tracker App.csproj", "Budget Tracker App/"]
RUN dotnet restore "Budget Tracker App/Budget Tracker App.csproj"
COPY . .
WORKDIR "/src/Budget Tracker App"
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final stage: runtime environment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["Budget Tracker App.exe"]
