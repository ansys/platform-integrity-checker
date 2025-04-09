# ------------------------------------------------------
    FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
    WORKDIR /app
    
    COPY . ./
    
    RUN dotnet restore pic.Web/pic.Web.csproj \
        -r linux-x64
    
    RUN dotnet publish pic.Web/pic.Web.csproj \
        --no-restore \
        -c Release \
        -r linux-x64 \
        -o publish \
        --no-self-contained
    
    # ------------------------------------------------------
    FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
    WORKDIR /app
    
    COPY --from=build /app/publish .
    EXPOSE 8080
    
    LABEL org.opencontainers.image.authors="Ansys PSI team <PlatformServicesandIntegration@ansys.com>"
    LABEL org.opencontainers.image.vendor="ANSYS Inc."
    LABEL org.opencontainers.image.title="pic frontend"
    LABEL org.opencontainers.image.description="Plateform Integrity Checker frontend"
    LABEL org.opencontainers.image.created=${CREATED_AT}
    LABEL org.opencontainers.image.revision=${REVISION}
    LABEL org.opencontainers.image.version=${VERSION}
    
    ENTRYPOINT ["./pic.Web"]