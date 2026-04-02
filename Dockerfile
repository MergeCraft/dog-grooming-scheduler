# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and package management files
COPY DogGrommingScheduler/Directory.Packages.props DogGrommingScheduler/
COPY DogGrommingScheduler/DogGrommingScheduler.slnx DogGrommingScheduler/

# Copy project files for restore
COPY DogGrommingScheduler/DogGrommingScheduler/WebAPI.csproj DogGrommingScheduler/DogGrommingScheduler/
COPY DogGrommingScheduler/AplicationLogic/AplicationLogic.csproj DogGrommingScheduler/AplicationLogic/
COPY DogGrommingScheduler/BusinessLogic/BusinessLogic.csproj DogGrommingScheduler/BusinessLogic/
COPY DogGrommingScheduler/DataAccess/DataAccess.csproj DogGrommingScheduler/DataAccess/
COPY DogGrommingScheduler/Shared/Shared.csproj DogGrommingScheduler/Shared/
COPY DogGrommingScheduler/AplicationLogic.Tests/AplicationLogic.Tests.csproj DogGrommingScheduler/AplicationLogic.Tests/

# Restore dependencies
RUN dotnet restore DogGrommingScheduler/DogGrommingScheduler/WebAPI.csproj

# Copy source code
COPY DogGrommingScheduler/DogGrommingScheduler/ DogGrommingScheduler/DogGrommingScheduler/
COPY DogGrommingScheduler/AplicationLogic/ DogGrommingScheduler/AplicationLogic/
COPY DogGrommingScheduler/BusinessLogic/ DogGrommingScheduler/BusinessLogic/
COPY DogGrommingScheduler/DataAccess/ DogGrommingScheduler/DataAccess/
COPY DogGrommingScheduler/Shared/ DogGrommingScheduler/Shared/

# Publish
RUN dotnet publish DogGrommingScheduler/DogGrommingScheduler/WebAPI.csproj \
    -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "WebAPI.dll"]
