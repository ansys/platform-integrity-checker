# ------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /app

COPY . ./

RUN dotnet restore pic.ApiService/pic.ApiService.csproj \
    -r linux-x64

RUN dotnet publish pic.ApiService/pic.ApiService.csproj \
    --no-restore \
    -c Release \
    -r linux-x64 \
    -o publish \
    --no-self-contained

# ------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .
EXPOSE 8080

LABEL org.opencontainers.image.authors="Ansys PSI team" \
    org.opencontainers.image.vendor="ANSYS Inc." \
    org.opencontainers.image.title="pic backend" \
    org.opencontainers.image.description="Plateform Integrity Checker backend" \
    org.opencontainers.image.created=${CREATED_AT} \
    org.opencontainers.image.revision=${REVISION} \
    org.opencontainers.image.version=${VERSION}

ENTRYPOINT ["./pic.ApiService"]
