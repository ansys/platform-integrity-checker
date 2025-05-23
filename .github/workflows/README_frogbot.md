# Frogbot security scan workflows

see https://github.com/ansys-internal/ci-templates/pull/176

## Status and TODO:

- PoC to run Frogbot on github.com/ansys organization repositories
- Connects to https://ansyscpp.jfrog.io SaaS instance - accessible from GitHub-hosted runners. This SaaS intance is temporary.
- TODO: connect to https://artifactory.ansys.com - requires self-hosted runners with internal network access. Ideally through runner scale sets hosted on Ansys-maintained k8s.
- TODO: run in container with verified Frogbot execution environment. Need package manager executeables.
- TODO: verify Frogbot support and performance with Docker, Docker compose.