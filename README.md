🛒 eCommerce Microservices Architecture with API Gateway
Overview
This project demonstrates a microservice-based architecture tailored for an eCommerce platform, using an API Gateway to streamline communication between clients and backend services.

Each microservice is responsible for a specific domain, such as:

Products Service: Manages product catalog operations

Orders Service: Handles order placement and processing

User Accounts Service: Manages user registration, authentication, and profiles

The API Gateway acts as a single entry point, routing client requests to appropriate services while also handling cross-cutting concerns like:

Request routing

Security enforcement

Rate limiting

Caching

Retry mechanisms for transient errors

Key Features
Microservices Architecture: Services are decoupled, independently deployable, and scalable.

API Gateway: Centralized management of external requests and internal service communication.

Rate Limiting: Protects services from abuse by controlling the number of requests a client can make.

Caching: Speeds up response times by storing frequently accessed data temporarily.

Retry Strategy: Improves resilience by automatically retrying failed service calls due to temporary issues.

Scalability & Resilience: Designed to handle high traffic and recover gracefully from failures.

Architecture Diagram
[Client Apps]
↓
[API Gateway (.NET)]
↓
┌───────────────┬─────────────────┬────────────────────┐
│ Products API │ Orders API │ User Accounts API │
│ (.NET + SQL) │ (.NET + SQL) │ (.NET + SQL) │
└───────────────┴─────────────────┴────────────────────┘

Technologies Used
Backend Framework: .NET 8 Web API

Database: SQL Server

API Gateway:

Ocelot API Gateway (commonly used with .NET)

Caching: In-memory (e.g., MemoryCache)

Authentication: (Planned/Future) JWT or OAuth2

Logging/Monitoring: Serilog

Getting Started
Clone the Repository

bash

git clone https://github.com/islammohmed/ECommerceApi.NETCore.git
cd ecommerce-microservices
Set Up Services

Each service has its own directory with Dockerfiles and setup scripts.

Configure environment variables (DB connections, ports, API keys).

Run the API Gateway

Set up routes and services.

Enable rate limiting and caching as per configuration.

Base URL: http://localhost:5001

API endpoints are routed through the API Gateway.

Best Practices Implemented
Fault isolation between services

Circuit breakers and retry logic

Asynchronous communication where needed

Monitoring and centralized logging (future enhancement)

Future Enhancements
Add service discovery with tools like Consul or Eureka

Implement OAuth 2.0 authentication

Integrate with a message broker for event-driven workflows

Add observability (Prometheus, Grafana)
