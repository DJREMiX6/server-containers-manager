# ---- Stage 1: backend build ----

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build
WORKDIR /src

COPY backend/ServerContainersManager.slnx ./
COPY backend/ServerContainerManager.API/*.csproj             ./ServerContainerManager.API/
COPY backend/ServerContainerManager.Application/*.csproj     ./ServerContainerManager.Application/
COPY backend/ServerContainerManager.Domain/*.csproj          ./ServerContainerManager.Domain/
COPY backend/ServerContainerManager.Shared.Utils/*.csproj  ./ServerContainerManager.Shared.Utils/
COPY backend/ServerContainersManager.ServiceDefaults/*.csproj  ./ServerContainersManager.ServiceDefaults/
COPY backend/ServerContainersManager.AppHost/*.csproj  ./ServerContainersManager.AppHost/

RUN dotnet restore

COPY backend/ .
RUN dotnet publish ServerContainerManager.API/ServerContainerManager.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---- Stage 2: frontend build ----
FROM node:22-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package.json frontend/pnpm-lock.yaml frontend/pnpm-workspace.yaml ./
RUN corepack enable && corepack prepare pnpm@latest --activate && pnpm install --frozen-lockfile
COPY frontend/ ./
ENV NX_DAEMON=false
ENV NODE_OPTIONS=--max-old-space-size=4096
RUN pnpx nx run build:production

# ---- Stage 3: runtime ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=backend-build /app/publish .
COPY --from=frontend-build /frontend/dist/apps/server-container-manager-frontend/browser ./wwwroot

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_HTTP_PORTS=8080
ENV ConnectionStrings__AppDb="Data Source=/app/data/app.sqlite"

VOLUME ["/app/data"]
EXPOSE 8080
EXPOSE 8443

# Runs as root by default (no USER set), 
# required for Docker.DotNet to access /var/run/docker.sock 
# regardless of internal UID.
ENTRYPOINT ["dotnet", "ServerContainerManager.API.dll"]
