# Build from repository root:

# docker build -f src/Reserve.Presentation.Api/Dockerfile --platform linux/amd64 -t reserve-api .
# docker run -p 8080:80 -it --platform linux/amd64 --name reserve-api reserve-api:latest

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY src/Reserve.Core/*.csproj src/Reserve.Core/
COPY src/Reserve.Application/*.csproj src/Reserve.Application/
COPY src/Reserve.Infrastructure.Data/*.csproj src/Reserve.Infrastructure.Data/
COPY src/Reserve.Presentation.Api/*.csproj src/Reserve.Presentation.Api/

RUN dotnet restore src/Reserve.Presentation.Api/Reserve.Presentation.Api.csproj

# Copy source code and build
COPY src/ src/
RUN dotnet publish src/Reserve.Presentation.Api/Reserve.Presentation.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=80

EXPOSE 80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Reserve.Presentation.Api.dll"]
