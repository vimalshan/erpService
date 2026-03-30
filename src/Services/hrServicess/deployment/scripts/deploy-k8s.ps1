# Kubernetes Deployment Script
# Deploys HR microservices to Kubernetes cluster

param(
    [string]$Action = "deploy",  # deploy, delete, status, logs, port-forward
    [string]$Service = $null,
    [string]$Namespace = "hr-microservices",
    [string]$Context = $null,
    [string]$Registry = "docker.io",
    [string]$ImageTag = "latest"
)

$ErrorActionPreference = "Stop"
$scriptPath = Split-Path -Parent $MyInvocation.MyCommand.Path
$k8sPath = Join-Path (Split-Path -Parent $scriptPath) "k8s"

Write-Host "HR Microservices Kubernetes Deployment" -ForegroundColor Green
Write-Host "=======================================" -ForegroundColor Green
Write-Host "Action: $Action" -ForegroundColor Cyan
Write-Host "Namespace: $Namespace`n" -ForegroundColor Cyan

# Check kubectl is available
try {
    $kubectlVersion = kubectl version --client --short 2>/dev/null
    Write-Host "kubectl available: $kubectlVersion" -ForegroundColor Yellow
}
catch {
    Write-Host "ERROR: kubectl is not installed or not in PATH" -ForegroundColor Red
    exit 1
}

# Set context if specified
if ($Context) {
    Write-Host "Switching to context: $Context" -ForegroundColor Cyan
    kubectl config use-context $Context
}

switch ($Action) {
    "deploy" {
        Write-Host "Creating namespace and deploying services..." -ForegroundColor Cyan
        
        # Create namespace
        kubectl create namespace $Namespace --dry-run=client -o yaml | kubectl apply -f -
        
        # Apply manifests in order
        $manifests = @(
            "00-infrastructure.yaml",
            "01-api-gateway.yaml",
            "02-microservices.yaml"
        )
        
        foreach ($manifest in $manifests) {
            $manifestPath = Join-Path $k8sPath $manifest
            if (Test-Path $manifestPath) {
                Write-Host "Applying $manifest..." -ForegroundColor Cyan
                kubectl apply -f $manifestPath --namespace=$Namespace
            }
            else {
                Write-Host "WARNING: Manifest not found: $manifest" -ForegroundColor Yellow
            }
        }
        
        Write-Host "Waiting for deployments to be ready..." -ForegroundColor Yellow
        Start-Sleep -Seconds 10
        kubectl rollout status deployment --all --namespace=$Namespace --timeout=10m
        
        Write-Host "Deployment completed successfully!" -ForegroundColor Green
    }
    
    "delete" {
        Write-Host "Deleting all services from namespace $Namespace..." -ForegroundColor Yellow
        $confirmation = Read-Host "Are you sure? (yes/no)"
        
        if ($confirmation -eq "yes") {
            kubectl delete namespace $Namespace --ignore-not-found=true
            Write-Host "Namespace deleted successfully!" -ForegroundColor Green
        }
        else {
            Write-Host "Deletion cancelled." -ForegroundColor Yellow
        }
    }
    
    "status" {
        Write-Host "Checking deployment status..." -ForegroundColor Cyan
        
        Write-Host "`nNamespace status:" -ForegroundColor Cyan
        kubectl get namespace $Namespace
        
        Write-Host "`nDeployments:" -ForegroundColor Cyan
        kubectl get deployments --namespace=$Namespace --output=wide
        
        Write-Host "`nPods:" -ForegroundColor Cyan
        kubectl get pods --namespace=$Namespace --output=wide
        
        Write-Host "`nServices:" -ForegroundColor Cyan
        kubectl get services --namespace=$Namespace --output=wide
        
        Write-Host "`nIngress:" -ForegroundColor Cyan
        kubectl get ingress --namespace=$Namespace --output=wide
    }
    
    "logs" {
        if (-not $Service) {
            Write-Host "ERROR: Service name required for logs action" -ForegroundColor Red
            Write-Host "Usage: deploy-k8s.ps1 -Action logs -Service <service-name>" -ForegroundColor Yellow
            exit 1
        }
        
        Write-Host "Streaming logs for $Service (press Ctrl+C to stop)..." -ForegroundColor Cyan
        kubectl logs deployment/$Service --namespace=$Namespace --follow --all-containers=true
    }
    
    "port-forward" {
        if (-not $Service) {
            $Service = "api-gateway"
        }
        
        $localPort = 8080
        Write-Host "Port forwarding to $Service:" -ForegroundColor Cyan
        Write-Host "Local: http://localhost:$localPort" -ForegroundColor Yellow
        Write-Host "Pod: Press Ctrl+C to stop" -ForegroundColor Yellow
        
        kubectl port-forward --namespace=$Namespace service/$Service $localPort:5310
    }
    
    "restart" {
        if (-not $Service) {
            Write-Host "Restarting all deployments..." -ForegroundColor Cyan
            kubectl rollout restart deployment --all --namespace=$Namespace
        }
        else {
            Write-Host "Restarting $Service..." -ForegroundColor Cyan
            kubectl rollout restart deployment/$Service --namespace=$Namespace
        }
        
        kubectl rollout status deployment --all --namespace=$Namespace
        Write-Host "Rollout completed!" -ForegroundColor Green
    }
    
    "scale" {
        if (-not $Service) {
            Write-Host "ERROR: Service name required for scale action" -ForegroundColor Red
            exit 1
        }
        
        $replicas = Read-Host "Number of replicas"
        Write-Host "Scaling $Service to $replicas replicas..." -ForegroundColor Cyan
        kubectl scale deployment/$Service --replicas=$replicas --namespace=$Namespace
    }
    
    "describe" {
        if (-not $Service) {
            Write-Host "ERROR: Service name required for describe action" -ForegroundColor Red
            exit 1
        }
        
        kubectl describe deployment/$Service --namespace=$Namespace
    }
    
    "shell" {
        if (-not $Service) {
            Write-Host "ERROR: Service name required for shell action" -ForegroundColor Red
            exit 1
        }
        
        Write-Host "Opening shell to $Service pod..." -ForegroundColor Cyan
        $pod = kubectl get pods --namespace=$Namespace -l app=$Service -o jsonpath='{.items[0].metadata.name}'
        kubectl exec -it $pod --namespace=$Namespace -- /bin/sh
    }
    
    default {
        Write-Host "ERROR: Unknown action '$Action'" -ForegroundColor Red
        Write-Host "Valid actions: deploy, delete, status, logs, port-forward, restart, scale, describe, shell" -ForegroundColor Yellow
        exit 1
    }
}

Write-Host "`nK8s deployment script completed." -ForegroundColor Green
