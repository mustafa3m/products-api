# Project: Cloud-Based Product API with AWS & Docker

## Table of Contents
1. [Project Overview](#project-overview)
2. [Project Directory Structure](#project-directory-structure)
3. [Task 1: Deploying API with Docker](#task-1-deploying-api-with-docker)
4. [Task 2: Pushing Docker Images to Docker Hub](#task-2-pushing-docker-images-to-docker-hub)
5. [Task 3: Deploy API to AWS EC2 with Nginx](#task-3-deploy-api-to-aws-ec2-with-nginx)
6. [Task 4: Migrating Database to AWS RDS](#task-4-migrating-database-to-aws-rds)
7. [Task 5: AWS CloudWatch Monitoring & Metrics](#task-5-aws-cloudwatch-monitoring--metrics)
8. [AWS Setup Instructions](#aws-setup-instructions)
    - [Creating a VPC](#creating-a-vpc)
    - [Launching an EC2 Instance](#launching-an-ec2-instance)
    - [Creating an RDS MySQL Instance](#creating-an-rds-mysql-instance)
    - [Configuring Security Groups](#configuring-security-groups)
9. [Testing the Application](#testing-the-application)
10. [Troubleshooting & Common Issues](#troubleshooting--common-issues)

---

## Project Overview
This project is a **microservice-based REST API** built using **.NET Core**, **MySQL**, **Nginx**, and **AWS Services** (EC2, RDS, CloudWatch). The API provides product management features, such as retrieving a list of products and fetching product details via **Docker containers**.

---

## Project Directory Structure
```
.
├── appsettings.json
├── Data
│   ├── Migrations
│   ├── ProductsDbContext.cs
├── database_setup.sql
├── docker-compose.yml
├── Dockerfile
├── Endpoints
│   ├── ProductsEndpoints.cs
├── Models
│   ├── Product.cs
├── nginx
│   ├── Dockerfile
│   ├── nginx.conf
├── products-api.csproj
├── products-api.sln
├── Program.cs
├── README.md
└── sql-scripts
```

---

## Task 1: Deploying API with Docker
### Steps:
1. **Build & Run API Container:**
   ```sh
   docker build -t products-api .
   docker run -p 8080:8080 products-api
   ```
2. **Run MySQL Database in Docker:**
   ```sh
   docker run --name catalog-db -e MYSQL_ROOT_PASSWORD=rootpass -e MYSQL_DATABASE=product_db -e MYSQL_USER=product-api -e MYSQL_PASSWORD=securepass -d mysql:8.0
   ```
3. **Run Nginx as Reverse Proxy:**
   ```sh
   docker-compose up -d
   ```
4. **Test API via Nginx:**
   ```sh
   curl http://localhost/api/products
   ```

---

## Task 2: Pushing Docker Images to Docker Hub
### Steps:
1. **Login to Docker Hub:**
   ```sh
   docker login
   ```
2. **Tag & Push API Image:**
   ```sh
   docker tag products-api mustafa3mm/products-api
   docker push mustafa3mm/products-api
   ```
3. **Tag & Push Nginx Image:**
   ```sh
   docker tag nginx-proxy mustafa3mm/nginx-proxy
   docker push mustafa3mm/nginx-proxy
   ```

---

## Task 3: Deploy API to AWS EC2 with Nginx
### Steps:
1. **Launch an EC2 Instance (Ubuntu 22.04)**
2. **SSH into Instance:**
   ```sh
   ssh -i product_mustafa.pem ubuntu@<EC2-PUBLIC-IP>
   ```
3. **Install Docker & Docker Compose:**
   ```sh
   sudo apt update && sudo apt install docker.io -y
   sudo systemctl enable --now docker
   ```
4. **Pull & Run API and Nginx Containers:**
   ```sh
   docker-compose up -d
   ```
5. **Test API:**
   ```sh
   curl http://<EC2-PUBLIC-IP>/api/products
   ```

---

## Task 4: Migrating Database to AWS RDS
### Steps:
1. **Create RDS MySQL Instance**
2. **Update `docker-compose.yml` to use RDS Endpoint**
3. **Restart API Container**
   ```sh
   docker-compose up -d --force-recreate
   ```
4. **Verify API Connectivity**
   ```sh
   curl http://<EC2-PUBLIC-IP>/api/products
   ```

---

## Task 5: AWS CloudWatch Monitoring & Metrics
### Steps:
1. **Install CloudWatch Agent on EC2:**
   ```sh
   sudo yum install -y amazon-cloudwatch-agent
   ```
2. **Configure CloudWatch Logs & Metrics:**
   ```sh
   sudo vim /opt/aws/amazon-cloudwatch-agent/etc/amazon-cloudwatch-agent.json
   ```
   ```json
   {
       "logs": {
           "logs_collected": {
               "files": {
                   "collect_list": [
                       {
                           "file_path": "/home/ubuntu/api_calls.log",
                           "log_group_name": "ProductApiLogs",
                           "log_stream_name": "ApiCallLogStream"
                       }
                   ]
               }
           }
       },
       "metrics": {
           "namespace": "ProductApiMetrics",
           "metrics_collected": {
               "log": {
                   "metric_name": "ApiCallCount",
                   "unit": "Count"
               }
           }
       }
   }
   ```
3. **Start CloudWatch Agent:**
   ```sh
   sudo systemctl restart amazon-cloudwatch-agent
   ```

---

## AWS Setup Instructions
### Creating a VPC
1. Go to AWS VPC Console → **Create VPC**
2. Configure Subnets & Security Groups

### Launching an EC2 Instance
1. Go to EC2 Console → **Launch Instance**
2. Select Ubuntu 22.04, t2.micro
3. Attach Security Groups (allow SSH, HTTP, and MySQL)

### Creating an RDS MySQL Instance
1. Go to AWS RDS Console → **Create Database**
2. Select MySQL (Free Tier)
3. Set DB Name: `product_db`, User: `product-api`, Password: `securepass`
4. Set security group to allow EC2 access

### Configuring Security Groups
- Allow **SSH (22)** from your IP
- Allow **HTTP (80)** from anywhere
- Allow **MySQL (3306)** from EC2 security group

---

## Testing the Application
1. **Test API Locally:**
   ```sh
   curl http://localhost/api/products
   ```
2. **Test API on EC2:**
   ```sh
   curl http://<EC2-PUBLIC-IP>/api/products
   ```
3. **Check CloudWatch Metrics:**
   - Go to AWS CloudWatch → Metrics → `ProductApiMetrics`

---

## Troubleshooting & Common Issues
- **Docker Container Not Running?**
  ```sh
  docker ps -a
  docker logs <container_id>
  ```
- **Cannot Connect to RDS?**
  - Ensure Security Group allows EC2 instance
  - Test connection:
    ```sh
    mysql -h your-rds-endpoint -u product-api -p
    ```

