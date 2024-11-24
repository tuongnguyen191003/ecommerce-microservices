# 🛒 **CyberCore Store: Build E-commerce Campaigns with Robust Microservices Architecture**

![CyberCore Store Logo](https://github.com/tuongnguyen191003/ecommerce-microservices/blob/main/assets/images/cybercore.logo.jpg)

Welcome to **CyberCore Store**, a cutting-edge e-commerce platform built for tech enthusiasts. Whether you're shopping for smartphones, laptops, or tablets, **CyberCore Store** combines seamless user experience with a scalable and high-performance backend powered by microservices architecture. 🌟

# 🚀 **About CyberCore Store**

CyberCore Store is a vision to redefine online shopping in Vietnam. Designed to harness the potential of Vietnam's booming digital economy, **CyberCore Store** integrates secure payments, real-time interactions, and marketing tools to empower businesses and enrich customer experiences.

# 🧩 **Microservices Overview**

| **Service**            | **Feature**                              | **Description**                                                                                         |
|-------------------------|-------------------------------------------|---------------------------------------------------------------------------------------------------------|
| **User Service**        | Registration/Login                       | Manage user accounts, secure JWT-based authentication, and session management.                         |
|                         | Profile Management                       | Update personal information, manage addresses for shipping.                                             |
| **Product Service**     | Catalog Management                       | Handle product listings, categories, pricing, and availability.                                         |
|                         | Reviews and Ratings                      | User feedback to enhance product transparency and trust.                                                |
| **Cart Service**        | Shopping Cart                            | Add, edit, or remove items in the user’s shopping cart.                                                 |
| **Order Service**       | Order Lifecycle                          | Manage order creation, payment, and status updates seamlessly.                                         |
| **Payment Service**     | Payment Gateway Integration              | Support for PayPal, VNPay, MOMO, and other payment gateways.                                            |
| **Inventory Service**   | Stock Management                         | Real-time inventory tracking for accurate processing.                                                   |
| **Shipping Service**    | Shipment Tracking                        | Calculate fees, update delivery statuses, and integrate third-party logistics.                          |
| **Notification Service**| Email and SMS Notifications              | Notify users about order status and promotional campaigns.                                              |
| **Review Service**      | Product Reviews                          | Manage customer reviews and ratings.                                                                   |
| **Live Chat Service**   | Real-Time Customer Support               | Enable communication through platforms like Subiz, Talkto, Facebook, or Zalo.                          |
| **Admin Service**       | Admin Dashboard                          | Analyze platform data, generate reports, and monitor user behavior for insights.                        |
| **Analytics Service**   | User Behavior and Revenue Tracking       | Detailed reporting on platform performance.                                                            |

🔗 **Communication**: Services communicate via **HTTP** (synchronous) and **RabbitMQ** (asynchronous).

---

# 🛠️ **Technology Stack**

## 🌐 **Frontend**
- **Vite** ⚡: Fast development and build tool.
- **Telerik UI for ASP.NET Core**: High-performance UI components.
- **Lazy Loading**: Optimized user experience through on-demand resource loading.

## 🖥️ **Backend**
- **ASP.NET Core .NET 8**: High-performance RESTful API.
- **Entity Framework Core**: ORM for database management.
- **Dapper**: Efficient handling of complex SQL queries.
- **JWT Authentication**: Secure user authentication.
- **Memcached**: High-speed data caching.

## 🗄️ **Database**
- **SQL Server on Docker**: Containerized database solution.

## 📩 **Messaging & Notifications**
- **RabbitMQ**: Asynchronous messaging.
- **Email Campaigns**: Integration for newsletters and promotions.

## 🌟 **SEO & Marketing**
- **SEO Optimization**: Enhanced search engine visibility.
- **Social Sharing**: Tools to promote on Facebook, Twitter, and more.

---

# 🏗️ **System Architecture**

![Architecture Overview](placeholder-architecture-image-link)

CyberCore Store leverages a microservice-based architecture to ensure modular development and scalability. Each service is independently deployable and communicates efficiently.

---

# 🗄️ **Database Schema**

![Database Schema](placeholder-database-image-link)

The database design is optimized for e-commerce requirements, including user data, orders, and inventory.

---

# 🔐 **Understanding JWT (JSON Web Token)**

## **What is JWT?**

![What is JWT?](https://github.com/tuongnguyen191003/ecommerce-microservices/blob/main/assets/images/what-is-jwt.png)

JWT is a compact and secure way to transmit information between parties. Its stateless design ensures scalability without needing session storage.

### Key Components:
1. **Header**: Contains token metadata, including the algorithm.
2. **Payload**: Includes claims about the user.
3. **Signature**: Validates the token's integrity.

## **How JWT Works**

![How JWT Works](https://github.com/tuongnguyen191003/ecommerce-microservices/blob/main/assets/images/how-jwt-works.png)

1. User logs in.
2. The server validates credentials and generates a JWT.
3. The JWT is sent to the client, which uses it for API requests.
4. The server verifies the JWT for every request.

---

# 🐳 **Dockerized Microservices**

## Key Features:
- Multi-container app definitions via `docker-compose.yml`.
- Automates container builds and service orchestration.
- Enhances consistency and deployment efficiency.

![Dockerized Services](https://github.com/tuongnguyen191003/ecommerce-microservices/blob/main/assets/images/Containerize-Docker-Images.png)

---

# 🔄 **CI/CD with GitHub Actions**

![CI/CD Pipeline](https://github.com/tuongnguyen191003/ecommerce-microservices/blob/main/assets/images/ci-cd.png)

Automated workflows ensure robust testing, build, and deployment pipelines.

---

# 📊 **Real-Time Monitoring**

## Monitoring Tools:
- **Prometheus**: Tracks system health and performance.
- **Grafana**: Provides dashboards and alerts.

![Monitoring Overview](https://github.com/tuongnguyen191003/ecommerce-microservices/blob/main/assets/images/videoframe_2813.png)

---

# 🌐 **Try CyberCore Store**

- **Website**: [Live Demo Link](#)
- **Grafana Dashboard**: [Performance Dashboard](#)

---

# 📜 **Contribute**

1. Fork the repository.
2. Create a branch: `git checkout -b feature-name`.
3. Commit changes: `git commit -m "Add feature name"`.
4. Push to your branch: `git push origin feature-name`.
5. Submit a pull request.

---

# 🙌 **Acknowledgements**

Special thanks to the **CyberCore** development team for their dedication.

---

> **Follow us** on [GitHub](#) and [LinkedIn](#) for updates.
