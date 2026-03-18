# FinyearAPI Kubernetes Deployment Guide

## Overview

The FinyearAPI is now fully containerized and ready for Kubernetes deployment. This guide walks through the deployment process with complete K8s manifests including Service, Ingress, HPA, RBAC, and more.

## Files Created

```
k8s/
├── configmap.yaml       ✅ Environment configuration
├── secret.yaml          ⚠️  Needs credential updates
├── deployment.yaml      ✅ 3-replica deployment with health checks
├── service.yaml         ✅ LoadBalancer + ClusterIP services
├── hpa.yaml            ✅ Horizontal Pod Autoscaler (3-10 replicas)
├── ingress.yaml        ✅ NGINX Ingress + NetworkPolicy
└── rbac.yaml           ✅ ServiceAccount, Role, RoleBinding, PDB
Dockerfile              ✅ Multi-stage Docker build
```

## Deployment Prerequisites

### 1. Update Secrets (CRITICAL)

Edit `k8s/secret.yaml` and replace placeholder values:

```yaml
ConnectionStrings__AdminDbConnection: "Server=sql-server-host;Database=ADMINDB;User Id=sa;Password=YourPassword123!;"
Jwt__SecretKey: "your-production-strength-secret-key-minimum-32-characters-required-here"
```

### 2. Build Docker Image

```bash
cd E:\ERPMicroservice\src\Services\adminServices\finyearServices

# Build the image
docker build -t finyear-api:latest .

# Optional: Push to registry (if using Docker Hub, Azure Container Registry, etc.)
docker push myregistry.azurecr.io/finyear-api:latest

# Update image in deployment.yaml if using remote registry:
# image: myregistry.azurecr.io/finyear-api:latest
```

### 3. Kubernetes Cluster Requirements

- Kubernetes 1.20+ (for autoscaling v2, ingress networking.k8s.io/v1)
- NGINX Ingress Controller (for Ingress support)
- Metrics Server (for HPA CPU/memory metrics)
- Cert-Manager (for TLS certificate management via Ingress)

```bash
# Install Metrics Server (if not present)
kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml

# Install NGINX Ingress Controller
helm repo add ingress-nginx https://kubernetes.github.io/ingress-nginx
helm repo update
helm install nginx-ingress ingress-nginx/ingress-nginx --namespace ingress-nginx --create-namespace

# Install Cert-Manager (for auto TLS)
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/download/v1.13.0/cert-manager.yaml
```

## Deployment Steps

### Step 1: Create Namespace and RBAC

```bash
kubectl apply -f k8s/rbac.yaml
```

**What this does:**
- Creates `finyear-api` namespace
- Creates `finyear-api` ServiceAccount
- Creates `finyear-api-role` with limited permissions
- Creates `finyear-api-rolebinding` binding role to service account
- Creates `PodDisruptionBudget` to maintain minimum 2 replicas during updates

### Step 2: Deploy Configuration and Secrets

```bash
# Apply ConfigMap (environment variables)
kubectl apply -f k8s/configmap.yaml

# Apply Secret (must be updated with real credentials first!)
kubectl apply -f k8s/secret.yaml
```

### Step 3: Deploy the Application

```bash
# Apply Deployment (3 replicas with auto-restart)
kubectl apply -f k8s/deployment.yaml
```

**Verification:**
```bash
# Check deployment status
kubectl get deployments -n finyear-api
kubectl get pods -n finyear-api

# Watch pod creation
kubectl get pods -n finyear-api -w

# Check pod logs
kubectl logs -f deployment/finyear-api -n finyear-api
```

### Step 4: Expose Services

```bash
# Apply Services (LoadBalancer + ClusterIP)
kubectl apply -f k8s/service.yaml
```

**Services created:**
- `finyear-api`: LoadBalancer service on port 80 → 5000, 443 → 5001
- `finyear-api-internal`: ClusterIP service for internal pod-to-pod communication

### Step 5: Setup Ingress and TLS (Optional)

Update `k8s/ingress.yaml` with your domain name:

```yaml
- host: your-domain.example.com  # Change this
  http:
    paths:
    - path: /
      pathType: Prefix
      backend:
        service:
          name: finyear-api
          port:
            number: 80
```

Then deploy:

```bash
# Apply Ingress and NetworkPolicy
kubectl apply -f k8s/ingress.yaml
```

### Step 6: Configure Auto-Scaling

```bash
# Apply HPA (auto-scale to 10 replicas if CPU > 70% or Memory > 80%)
kubectl apply -f k8s/hpa.yaml
```

**Verification:**
```bash
kubectl get hpa -n finyear-api
kubectl get hpa -n finyear-api -w  # Watch autoscaling
```

## Complete Deployment Script

```bash
#!/bin/bash
set -e

echo "🚀 Starting FinyearAPI Kubernetes Deployment..."

echo "✅ Step 1: Creating namespace and RBAC..."
kubectl apply -f k8s/rbac.yaml

echo "✅ Step 2: Deploying ConfigMap and Secret..."
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.yaml

echo "✅ Step 3: Deploying FinyearAPI..."
kubectl apply -f k8s/deployment.yaml

echo "✅ Step 4: Exposing services..."
kubectl apply -f k8s/service.yaml

echo "✅ Step 5: Configuring Ingress and Network Policy..."
kubectl apply -f k8s/ingress.yaml

echo "✅ Step 6: Setting up auto-scaling..."
kubectl apply -f k8s/hpa.yaml

echo "⏳ Waiting for pods to be ready..."
kubectl wait --for=condition=Ready pod -l app=finyear-api -n finyear-api --timeout=300s

echo "✅ Deployment Complete!"
echo ""
echo "📊 Pod Status:"
kubectl get pods -n finyear-api

echo ""
echo "🔗 Access Information:"
LOAD_BALANCER_IP=$(kubectl get svc finyear-api -n finyear-api -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
echo "LoadBalancer IP: $LOAD_BALANCER_IP"
echo "API URL: http://$LOAD_BALANCER_IP/api/FinancialYear"
```

## Verification Steps

### 1. Check Pod Status

```bash
# Check all pods
kubectl get pods -n finyear-api
kubectl get pods -n finyear-api -o wide  # Shows node distribution

# Expected output (3 pods on different nodes due to anti-affinity):
# NAME                          READY   STATUS    RESTARTS   AGE     IP              NODE
# finyear-api-xyz123-abc        1/1     Running   0          2m      10.244.1.5      worker-1
# finyear-api-xyz123-def        1/1     Running   0          2m      10.244.2.6      worker-2
# finyear-api-xyz123-ghi        1/1     Running   0          2m      10.244.3.7      worker-3
```

### 2. Check Service Status

```bash
# Get LoadBalancer IP
kubectl get svc -n finyear-api
kubectl get svc finyear-api -n finyear-api -o jsonpath='{.status.loadBalancer.ingress[0].ip}'

# For Local/Minikube:
kubectl port-forward svc/finyear-api 8080:80 -n finyear-api
# Then access: http://localhost:8080/api/FinancialYear
```

### 3. Test API Endpoint

```bash
# Get service IP (for LoadBalancer)
EXTERNAL_IP=$(kubectl get svc finyear-api -n finyear-api -o jsonpath='{.status.loadBalancer.ingress[0].ip}')

# Test endpoint
curl http://$EXTERNAL_IP/api/FinancialYear

# Expected response (4 sample records):
# [
#   {"id":1,"name":"FY 2024-2025",...},
#   ...
# ]
```

### 4. Check Pod Logs

```bash
# View logs from specific pod
kubectl logs <pod-name> -n finyear-api

# View logs from all replicas
kubectl logs -f -l app=finyear-api -n finyear-api

# Example:
# info: FinyearAPI.Service.FinancialYearService[0]
#       Getting all financial years
# info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
#       Request finished HTTP/1.1 GET http://localhost:5000/api/FinancialYear
```

### 5. Check Resource Usage

```bash
# Current resource usage
kubectl top nodes
kubectl top pods -n finyear-api

# Expected (3 pods using ~50-100Mi memory each, 100-200m CPU):
# NAME                          CPU(cores)   MEMORY(bytes)
# finyear-api-xyz123-abc        150m         85Mi
# finyear-api-xyz123-def        120m         78Mi
# finyear-api-xyz123-ghi        140m         82Mi
```

### 6. Check Health Probes

```bash
# Describe pod to see probe status
kubectl describe pod <pod-name> -n finyear-api

# Look for:
# Liveness probe: http-get delay=30s timeout=5s period=10s #success=1 #failure=3
# Readiness probe: http-get delay=10s timeout=3s period=5s #success=1 #failure=2
# Startup probe: http-get delay=5s timeout=3s period=5s #success=1 #failure=30
```

### 7. Check HPA Status

```bash
# HPA current metrics
kubectl get hpa -n finyear-api
kubectl describe hpa finyear-api-hpa -n finyear-api

# Monitor autoscaling
kubectl get hpa -n finyear-api -w
```

### 8. Load Test (Simulate Scaling)

```bash
# Install Apache Bench
apt-get install apache2-utils  # Linux
choco install apache2          # Windows

# Run load test
ab -n 10000 -c 100 http://$EXTERNAL_IP/api/FinancialYear

# Watch pods increase
kubectl get hpa -n finyear-api -w
kubectl get pods -n finyear-api -w
```

## Troubleshooting

### Pods not starting?

```bash
# Check pod status
kubectl get pods -n finyear-api
kubectl describe pod <pod-name> -n finyear-api

# Check logs for errors
kubectl logs <pod-name> -n finyear-api --previous
```

### Common Issues:

| Issue | Solution |
|-------|----------|
| ImagePullBackOff | Check Docker image is built and available (docker images) |
| CrashLoopBackOff | Check logs for startup errors (kubectl logs) |
| Pending | Check node resources (kubectl top nodes, kubectl describe node) |
| LoadBalancer stuck on Pending | Normal for local/minikube (use port-forward instead) |
| Service unreachable | Check service endpoints: `kubectl get endpoints finyear-api -n finyear-api` |
| Database connection errors | Verify secret values match actual database credentials |
| Health check fails | Ensure /api/FinancialYear endpoint is responding |

### Database Connection Issues?

```bash
# Test from inside pod
kubectl exec -it <pod-name> -n finyear-api -- bash

# Inside pod:
curl http://localhost:5000/api/FinancialYear
# If this works locally but service doesn't respond externally,
# it's a networking/service issue, not app issue
```

## Cleanup

```bash
# Delete entire namespace and all resources
kubectl delete namespace finyear-api

# Or delete individual resources
kubectl delete deployment finyear-api -n finyear-api
kubectl delete svc finyear-api finyear-api-internal -n finyear-api
kubectl delete ingress finyear-api-ingress -n finyear-api
kubectl delete hpa finyear-api-hpa -n finyear-api
```

## Production Checklist

- [ ] Secret values updated with actual database credentials
- [ ] Docker image built and stored in registry
- [ ] Domain name configured in Ingress manifest
- [ ] TLS certificate issuer configured (cert-manager)
- [ ] Metrics Server installed on cluster
- [ ] NGINX Ingress Controller installed
- [ ] Pod resource requests/limits adjusted for your workload
- [ ] Database backup configured
- [ ] Monitoring and logging setup (Prometheus, Grafana, ELK)
- [ ] Network policies reviewed and applied
- [ ] RBAC permissions reviewed and minimized
- [ ] Pod Disruption Budget tested
- [ ] Disaster recovery plan documented

## Related Documentation

- [Kubernetes Deployment](https://kubernetes.io/docs/concepts/workloads/controllers/deployment/)
- [Ingress NGINX Documentation](https://kubernetes.github.io/ingress-nginx/)
- [Horizontal Pod Autoscaler](https://kubernetes.io/docs/tasks/run-application/horizontal-pod-autoscale/)
- [RBAC Authorization](https://kubernetes.io/docs/reference/access-authn-authz/rbac/)
- [Network Policies](https://kubernetes.io/docs/concepts/services-networking/network-policies/)
- [Pod Disruption Budgets](https://kubernetes.io/docs/tasks/run-application/configure-pdb/)
