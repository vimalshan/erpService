@echo off
REM ============================================================
REM ERP Microservices - Kubernetes Deploy Script
REM ============================================================

set ACTION=%1
if "%ACTION%"=="" set ACTION=apply

if "%ACTION%"=="apply" (
    echo Deploying ERP Microservices to Kubernetes...
    echo.
    echo [1/5] Creating namespace...
    kubectl apply -f k8s/namespace.yaml

    echo [2/5] Applying secrets...
    kubectl apply -f k8s/secrets.yaml

    echo [3/5] Applying config...
    kubectl apply -f k8s/configmap.yaml

    echo [4/5] Deploying infrastructure...
    kubectl apply -f k8s/infrastructure.yaml

    echo Waiting for infrastructure to be ready...
    kubectl -n erp-system wait --for=condition=ready pod -l app=sqlserver --timeout=120s
    kubectl -n erp-system wait --for=condition=ready pod -l app=rabbitmq --timeout=60s

    echo [5/5] Deploying services and API Gateway...
    kubectl apply -f k8s/services.yaml

    echo.
    echo Applying ingress...
    kubectl apply -f k8s/ingress.yaml

    echo.
    echo Waiting for API Gateway to be ready...
    kubectl -n erp-system wait --for=condition=ready pod -l app=api-gateway --timeout=120s

    echo.
    echo ============================================================
    echo Deployment complete! Check status:
    echo   kubectl -n erp-system get pods
    echo   kubectl -n erp-system get services
    echo.
    echo API Gateway: http://erp.local (via Ingress)
    echo   Health:  http://erp.local/health
    echo   Swagger: http://erp.local/swagger
    echo ============================================================
    goto :eof
)

if "%ACTION%"=="delete" (
    echo Removing ERP Microservices from Kubernetes...
    kubectl delete -f k8s/ingress.yaml --ignore-not-found
    kubectl delete -f k8s/services.yaml --ignore-not-found
    kubectl delete -f k8s/infrastructure.yaml --ignore-not-found
    kubectl delete -f k8s/configmap.yaml --ignore-not-found
    kubectl delete -f k8s/secrets.yaml --ignore-not-found
    kubectl delete -f k8s/namespace.yaml --ignore-not-found
    echo Done.
    goto :eof
)

if "%ACTION%"=="status" (
    echo === Pods ===
    kubectl -n erp-system get pods -o wide
    echo.
    echo === Services ===
    kubectl -n erp-system get services
    echo.
    echo === Ingress ===
    kubectl -n erp-system get ingress
    goto :eof
)

echo Usage: deploy-k8s.cmd [apply^|delete^|status]
echo   apply   - Deploy all resources (default)
echo   delete  - Remove all resources
echo   status  - Show cluster status
