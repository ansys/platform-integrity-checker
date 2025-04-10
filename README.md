# Platform Integrity Checker

Platform Integrity Checker (PIC) allows to assess deployment integrity:
- Ensure routing is correctly configured by exposing a fully qualified domain name through the API Gateway service.
- Ensure authentication is correctly configured by exercising IAM service.
- Ensure observability is correctly configured by exporting both frontend and backend telemetry to o11y service.

## Build

```sh
export CREATED_AT=$(date --rfc-3339=seconds)
export REVISION=$(git log -1 | head -1 | cut -d ' ' -f 2)
export VERSION='1.0.0'

docker compose \
    build \
    --build-arg "CREATED_AT=$CREATED_AT" \
    --build-arg "REVISION=$REVISION" \
    --build-arg "VERSION=$VERSION"
```

## Usage

```sh
export VERSION='1.0.0'

docker compose up
```

Browse [frontend](http://localhost:5000) and explore [telemetry](http://localhost:18888).
