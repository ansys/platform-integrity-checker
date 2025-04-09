#!/bin/bash
# ©2022, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.
version=$(awk '/^version:/ {print $2}' $(dirname "$0")/Chart.yaml)
set -e

scriptFolder=$(realpath $(dirname "$0"))

cd "$scriptFolder"

echo $TOKEN | helm registry login azwepsifujiaksacr.azurecr.io --username 00000000-0000-0000-0000-000000000000 --password-stdin

helm package --app-version $version .

helm push \
    pic-backend-$version.tgz \
    oci://azwepsifujiaksacr.azurecr.io/ansys/pic/helm