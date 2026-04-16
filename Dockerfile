# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ws002/ws002.csproj ws002/
RUN dotnet restore ws002/ws002.csproj

# Copy source code and build
COPY ws002/ ws002/
RUN dotnet build ws002/ws002.csproj -c Release -o /app/build --no-restore

# Publish stage
FROM build AS publish
RUN dotnet publish ws002/ws002.csproj -c Release -o /app/publish --no-build /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y --no-install-recommends curl && \
    rm -rf /var/lib/apt/lists/*

# Create non-root user
RUN addgroup --system --gid 1001 appgroup && \
    adduser --system --uid 1001 appuser --ingroup appgroup

# Copy published files
COPY --from=publish /app/publish .

# Set ownership
RUN chown -R appuser:appgroup /app

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 7000

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD curl -f http://localhost:7000/health || exit 1

# Set environment variables
ENV ASPNETCORE_URLS=http://+:7000
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "ws002.dll"]
