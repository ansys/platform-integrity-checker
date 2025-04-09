# Deployment

## Production

Setup context

```sh
kubectl config set-context pic \
    --namespace=pic-dev \
    --cluster=fuji-aws  \
    --user=fuji-oidc
kubectl config use-context pic
kubectl config get-contexts 
```

Deploy o11y

```sh
helm install \
    aspire \
    oci://azwepsifujiaksacr.azurecr.io/third_party/aspire \
    -f ./values.aspire.yaml
```

Deploy backend

```sh
helm install \
    pic-backend \
    oci://azwepsifujiaksacr.azurecr.io/ansys/pic/helm/pic-backend
```

Deploy backend

```sh
helm install \
    pic-frontend \
    oci://azwepsifujiaksacr.azurecr.io/ansys/pic/helm/pic-frontend
```

Browse [https://pic.dev.ansysapis.com/](https://pic.dev.ansysapis.com/).
Explore [https://aspire.pic.dev.ansysapis.com/](https://aspire.pic.dev.ansysapis.com/).