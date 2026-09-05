# Server Containers Manager

Personal server containers manager with RBAC.

[![Build and publish image](https://github.com/DJREMiX6/server-containers-manager/actions/workflows/publish.yml/badge.svg)](https://github.com/DJREMiX6/server-containers-manager/actions/workflows/publish.yml)
[![GHCR](https://img.shields.io/badge/ghcr.io-server--containers--manager-blue)](https://github.com/DJREMiX6/server-containers-manager/pkgs/container/server-containers-manager)
[![License: MIT](https://img.shields.io/badge/license-MIT-green)](LICENSE)

Backend (ASP.NET) and frontend (Angular) shipped as a single Docker image, backed by an embedded SQLite database. No external database, no separate frontend server, no orchestration beyond `docker run` — the container is the entire deployment unit, and its state persists in one Docker volume.

## Features

- Role-based access control (RBAC) for authentication and authorization.
- Manages Docker containers on the host via the Docker Engine API — start, stop, inspect, and control containers running on the same machine.
- Single-container deployment: API and Angular frontend are served from the same origin and port; no reverse proxy required to get running.
- SQLite storage embedded in the container, persisted through a single named volume.
- HTTPS is optional and configured entirely at deploy time — the image itself makes no assumption about TLS.

## Requirements

- Docker Engine (or Docker Desktop) on the host.
- Access to `/var/run/docker.sock` on that host — **required**, not optional, since container management is a core feature (see [Security note](#security-note-docker-socket-access) below).

## Quick start

### 1. Pull the image

```bash
docker pull ghcr.io/djremix6/server-containers-manager:latest
```

### 2. Create a persistent volume

```bash
docker volume create scm_data
```

### 3. Run the container

```bash
docker run -d --name scm \
  -p 8080:8080 \
  -v scm_data:/app/data \
  -v /var/run/docker.sock:/var/run/docker.sock \
  ghcr.io/djremix6/server-containers-manager:latest
```

Open `http://localhost:8080`.

### Docker Compose, equivalent

```yaml
services:
  scm:
    image: ghcr.io/djremix6/server-containers-manager:latest
    container_name: scm
    restart: unless-stopped
    ports:
      - "8080:8080"
    volumes:
      - scm_data:/app/data
      - /var/run/docker.sock:/var/run/docker.sock

volumes:
  scm_data:
```

```bash
docker compose up -d
```

## Configuration

All configuration is via environment variables — no config file to mount for basic use.

| Variable | Default (baked into image) | Purpose |
|---|---|---|
| `ASPNETCORE_ENVIRONMENT` | `Production` | ASP.NET Core environment name. |
| `ASPNETCORE_HTTP_PORTS` | `8080` | Port Kestrel listens on for HTTP. |
| `ConnectionStrings__AppDb` | `Data Source=/app/data/app.db` | SQLite connection string. Change only if you also change the data volume's mount path. |
| `ASPNETCORE_URLS` | unset | Set to include an `https://` binding to enable HTTPS (see below). |
| `ASPNETCORE_Kestrel__Certificates__Default__Path` | unset | Path to a `.pfx` certificate, if HTTPS is enabled. |
| `ASPNETCORE_Kestrel__Certificates__Default__Password` | unset | Password for the above certificate. |

### Enabling HTTPS (optional)

HTTPS is off by default (HTTP only, port 8080). To enable it, mount a certificate and set the corresponding environment variables — the app detects the HTTPS binding at startup and only then applies HTTPS redirection:

```bash
docker run -d --name scm \
  -p 8080:8080 -p 8443:8443 \
  -v scm_data:/app/data \
  -v /var/run/docker.sock:/var/run/docker.sock \
  -v /host/path/cert.pfx:/https/cert.pfx:ro \
  -e ASPNETCORE_URLS="http://+:8080;https://+:8443" \
  -e ASPNETCORE_Kestrel__Certificates__Default__Path=/https/cert.pfx \
  -e ASPNETCORE_Kestrel__Certificates__Default__Password=<pfx-password> \
  ghcr.io/djremix6/server-containers-manager:latest
```

If you terminate TLS at an external reverse proxy instead, leave this unset and only publish port 8080 internally.

## Data persistence & backup

Everything that needs to survive a container recreation (the SQLite database) lives under `/app/data`, mapped to the `scm_data` volume above. Recreating or updating the container (see below) never touches this volume.

Back it up with:

```bash
docker run --rm -v scm_data:/data -v "$PWD":/backup alpine \
  tar czf /backup/scm-data-backup.tar.gz -C /data .
```

Restore into a fresh volume:

```bash
docker run --rm -v scm_data:/data -v "$PWD":/backup alpine \
  tar xzf /backup/scm-data-backup.tar.gz -C /data
```

## Updating

```bash
docker pull ghcr.io/djremix6/server-containers-manager:latest
docker rm -f scm
docker run -d --name scm \
  -p 8080:8080 \
  -v scm_data:/app/data \
  -v /var/run/docker.sock:/var/run/docker.sock \
  ghcr.io/djremix6/server-containers-manager:latest
```

The named volume is untouched by this — only the container is replaced.

## Image tags

- `latest` — most recent successful build from `main`.
- `sha-<shortsha>` — immutable tag for a specific commit, useful for pinning a deployment or rolling back to a known-good build instead of tracking `latest`.

## Security note: Docker socket access

This application mounts and uses the host's Docker socket (`/var/run/docker.sock`) to manage containers. Access to that socket is equivalent to root access on the host — a process that can talk to it can create a privileged container and escape to the host, regardless of the user it runs as inside its own container. This is inherent to any tool with this feature (Portainer, Yacht, etc. included), not specific to a misconfiguration here.

Practical implications:
- Don't expose this container's port to the public internet without authentication in front of it (the app's own RBAC handles this once you're past initial setup — don't skip configuring it).
- Only run this on hosts where you're comfortable with the container having effective root control.

## License

MIT — see [LICENSE](LICENSE).