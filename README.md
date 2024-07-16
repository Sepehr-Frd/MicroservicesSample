This is my sample of Microservices Architecture using the following technologies:
- .NET (C#)
- SQL Server
- MongoDB
- gRPC
- RabbitMQ
- Docker
- Kubernetes

This is how it works:
ToDoListManager application can be used to manage to-do lists which uses SQL Server as its primary database and MongoDB as its log database.
ChangeDataCaptureHub application is kind of a to-do items backup manager.
As soon as ChangeDataCaptureHub starts working, it fetches any new to-do item it currently doesn't have stored in database (MongoDB) using gRPC.
Then it keeps listening on a RabbitMQ queue and when you create a to-do item the ChangeDataCaptureHub application will asynchronously get the new to-do item and adds it to the database (MongoDB).

This is how you can get it up and running:
  1. You need to have Kubernetes installed on your machine (using Docker Desktop, Podman, etc.) as well as kubectl (Kubernetes command line tool)
  2. Go to ToDoListManager and ChangeDataCaptureHub directories and build the 'Dockerfile's
  3. Go to K8S directory and change image names of the ToDoListManager and ChangeDataCaptureHub deployments to the name you set in the previous step
  4. Add localhost.net to your operating system's hosts so that it would get resolved as 127.0.0.1
  5. Open a terminal window in the K8S directory and run the following commands (the order matters):
			kubectl apply -f Deployments-1
			kubectl apply -f Deployments-2
			kubectl apply -f Deployments-3
			kubectl apply -f Deployments-4
  6. Then run 
  			kubectl get pods
	to make sure everything is working
  7. You can now access ToDoListManager application swagger page on 'localhost.net/swagger'
