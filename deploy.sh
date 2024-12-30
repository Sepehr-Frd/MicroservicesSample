#!/bin/bash

# Function to get the pod name
get_pod_name() {
  local deployment_name=$1
  kubectl get pods --selector=app=mssql -o jsonpath='{.items[0].metadata.name}'
}

# Function to check if RabbitMQ is ready
check_rabbitmq() {
  local service_name=$1
  local host=$2
  local port=$3
  local username=$4
  local password=$5
  echo "Checking $service_name at $host:$port..."
  
	while ! curl -s -u "$username:$password" http://$host:$port/api/health/checks/virtual-hosts | grep "\"status\":\"ok\"" > /dev/null; do
    	echo "$service_name is not ready. Retrying in 5 seconds..."
    	sleep 5
  	done
  echo "$service_name is ready!"
}

# Function to check if SQL Server is ready
check_sqlserver() {
  local service_name=$1
  local deployment_name=$2
  local host=$3
  local port=$4
  local username=$5
  local password=$6

  echo "Fetching the pod name for deployment $deployment_name..."
  local pod_name=$(get_pod_name $deployment_name)

  if [ -z "$pod_name" ]; then
    echo "Error: Unable to find a pod for deployment $deployment_name"
    exit 1
  fi

  echo "Checking $service_name in pod $pod_name at $host:$port..."

  while ! kubectl exec -it $pod_name -- /opt/mssql-tools18/bin/sqlcmd -S $host,$port -U $username -P $password -C -t 60 -Q "SELECT @@VERSION" >/dev/null 2>&1; do
    echo "$service_name is not ready. Retrying in 5 seconds..."
    sleep 5
  done

  echo "$service_name is ready!"
}

check_mongodb() {
  local service_name=$1
  local host=$2
  local port=$3
  local username=${4:-""}  # Optional username
  local password=${5:-""}  # Optional password

  echo "Checking $service_name at $host:$port..."

  while true; do
    if command -v mongosh >/dev/null 2>&1; then
      # Use mongosh for newer MongoDB versions
      if [ -n "$username" ] && [ -n "$password" ]; then
        mongosh --host $host --port $port --username $username --password $password --eval "db.adminCommand('ping')" >/dev/null 2>&1
      else
        mongosh --host $host --port $port --eval "db.adminCommand('ping')" >/dev/null 2>&1
      fi
    elif command -v mongo >/dev/null 2>&1; then
      # Use mongo for older MongoDB versions
      if [ -n "$username" ] && [ -n "$password" ]; then
        mongo --host $host --port $port --username $username --password $password --eval "db.adminCommand('ping')" >/dev/null 2>&1
      else
        mongo --host $host --port $port --eval "db.adminCommand('ping')" >/dev/null 2>&1
      fi
    else
      echo "Error: Neither 'mongosh' nor 'mongo' command is available. Please install a MongoDB CLI tool."
      exit 1
    fi

    if [ $? -eq 0 ]; then
      break
    else
      echo "$service_name is not ready. Retrying in 5 seconds..."
      sleep 5
    fi
  done

  echo "$service_name is ready!"
}

# Function to check if an HTTP endpoint is up
wait_for_http() {
  local service_name=$1
  local url=$2

  echo "Waiting for $service_name at $url..."
  while ! curl -s --head "$url" | grep "200 OK" > /dev/null; do
    echo "$service_name is not ready yet. Retrying in 5 seconds..."
    sleep 5
  done
  echo "$service_name is ready!"
}

set -e  # Exit immediately if a command exits with a non-zero status
set -o pipefail  # Prevents errors in a pipeline from being masked

echo "Building Docker images..."

cd ./ToDoListManager
docker build -t local/to-do-list-manager-web:1.0.0 --file Dockerfile .
cd ..

cd ./ChangeDataCaptureHub
docker build -t local/change-data-capture-hub-web:1.0.0 --file Dockerfile .
cd ..

echo "Dependencies are ready. Applying Kubernetes deployments..."

# Apply Kubernetes deployments
kubectl apply -f ./K8S/Deployment-1
kubectl apply -f ./K8S/Deployment-2

echo "Checking dependencies..."

# Wait for RabbitMQ
check_rabbitmq "RabbitMQ" "localhost" 15672 "guest" "guest"

# Wait for SQL Server
check_sqlserver "SQL Server" "mssql" "localhost" "1433" "sa" "YourSAP@55word"

# Wait for MongoDB
check_mongodb "MongoDB" "localhost" 27017

kubectl apply -f ./K8S/Deployment-3

# Wait for ToDoListManager to be ready
wait_for_http "ToDoListManager" "http://localhost.net:80/healthcheck"

kubectl apply -f ./K8S/Deployment-4

echo "Script completed successfully!"