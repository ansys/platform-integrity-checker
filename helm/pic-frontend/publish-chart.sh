#!/bin/bash
# ©2025, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.
version=$(awk '/^version:/ {print $2}' $(dirname "$0")/Chart.yaml)
set -e
scriptFolder=$(realpath $(dirname "$0"))

TOKEN=$(az acr login --name azwepsifujiaksacr --subscription 92515446-26a8-43e6-8b5e-effd89c7e563 --expose-token --output tsv --query accessToken)

echo $TOKEN | helm registry login azwepsifujiaksacr.azurecr.io --username 00000000-0000-0000-0000-000000000000 --password-stdin

cd "$scriptFolder"

helm package --app-version $version .

helm push \
    pic-frontend-$version.tgz \
    oci://azwepsifujiaksacr.azurecr.io/ansys/pic/helm
