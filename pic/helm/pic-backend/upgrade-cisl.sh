#!/bin/bash
# ©2022, ANSYS Inc. Unauthorized use, distribution or duplication is prohibited.

set -e

scriptFolder=$(realpath $(dirname "$0"))

cd "$(git rev-parse --show-toplevel)"

# TODO: temporary workflow before having a common workflow for publication
# Run a bazel run //ansys/fuji/spaces/backend:push_image_latest to publish the backend image to ACR before upgrading the helm chart.

helm dependency update $scriptFolder

helm lint \
     $scriptFolder \
     -f $scriptFolder/values.yaml \
     -f $scriptFolder/values-cisl.yaml

helm upgrade \
     --install \
     --reset-values \
     pic-backend \
     $scriptFolder \
     --namespace pic \
     --create-namespace \
     --kube-context fuji-test-admin \
     -f $scriptFolder/values-cisl.yaml \
     "${@}"