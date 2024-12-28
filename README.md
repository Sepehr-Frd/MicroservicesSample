# Microservices Architecture Sample

This is my sample implementation of a Microservices Architecture using the following technologies:

- **.NET (C#)**
- **SQL Server**
- **MongoDB**
- **gRPC**
- **RabbitMQ**
- **Docker**
- **Kubernetes**

## Overview

### ToDoListManager Application

The **ToDoListManager** application is used to manage to-do lists. It utilizes:

- **SQL Server** as its primary database.
- **MongoDB** as its log database.

### ChangeDataCaptureHub Application

The **ChangeDataCaptureHub** application acts as a to-do item backup manager. Here's how it works:

1. **Startup Sync**: When the application starts, it fetches any new to-do items not already stored in its database (**MongoDB**) using **gRPC**.
2. **Asynchronous Updates**: It listens on a **RabbitMQ queue** for new to-do items. Whenever a new to-do item is created, the application asynchronously retrieves it and stores it in its database (**MongoDB**).

---

## Getting Started

Follow these steps to get the system up and running:

1. Ensure Kubernetes is installed on your machine. You can use Docker Desktop, Podman, or another Kubernetes provider.
2. Install `kubectl`, the Kubernetes command-line tool.
3. Add `localhost.net` to your operating system's hosts file so it resolves to `127.0.0.1`.
4. Open a terminal in the Microservices directory and make the deployment script executable by running:
   ```bash
   chmod +x deploy.sh
   ```
5. Deploy the services by running:
   ```bash
   ./deploy.sh
   ```
6. Once everything is up and running, you can access the **ToDoListManager** application scalar page at:  
   [http://localhost.net/scalar/v1](http://localhost.net/scalar/v1)
