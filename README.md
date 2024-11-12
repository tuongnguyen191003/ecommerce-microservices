# 🛍️ E-commerce Microservices Platform

Nền tảng thương mại điện tử hiện đại với kiến trúc microservices, sử dụng ASP.NET Core và Docker để cung cấp giải pháp mạnh mẽ, linh hoạt, và dễ dàng triển khai. Hệ thống tích hợp CI/CD với GitHub Actions và giám sát bằng Prometheus & Grafana.

---

## 📑 Chi tiết các Microservice

| 🧩 **Microservice**          | 🛠️ **Chức năng chính**                                                                                   | 🔗 **Giao tiếp** |
|------------------------------|----------------------------------------------------------------------------------------------------------|------------------|
| **User Service**             | Quản lý người dùng: đăng ký, đăng nhập, quản lý hồ sơ và địa chỉ giao hàng.                             | HTTP             |
| **Product Service**          | Quản lý sản phẩm và danh mục sản phẩm, đánh giá sản phẩm.                                               | HTTP             |
| **Cart Service**             | Quản lý giỏ hàng của người dùng: thêm, xóa, cập nhật sản phẩm trong giỏ hàng.                           | HTTP             |
| **Order Service**            | Xử lý đơn hàng: tạo, cập nhật trạng thái, lịch sử đơn hàng.                                             | HTTP, RabbitMQ   |
| **Payment Service**          | Xử lý thanh toán qua các cổng thanh toán trực tuyến hoặc COD, xử lý hoàn tiền khi đơn hàng bị hủy.      | HTTP             |
| **Inventory Service**        | Quản lý kho hàng, cập nhật tồn kho khi có đơn hàng mới.                                                 | HTTP             |
| **Shipping Service**         | Tính phí vận chuyển và theo dõi trạng thái giao hàng, tích hợp với đơn vị vận chuyển bên thứ ba.       | HTTP             |
| **Notification Service**     | Gửi thông báo qua email hoặc SMS về trạng thái đơn hàng, tích hợp với SendGrid, Mailgun, và Twilio.    | RabbitMQ         |
| **Review & Rating Service**  | Đánh giá sản phẩm, hiển thị đánh giá của người dùng.                                                    | HTTP             |
| **Admin Service**            | Quản trị hệ thống: quản lý người dùng, sản phẩm, đơn hàng, báo cáo và phân tích.                       | HTTP             |
| **Analytics Service**        | Theo dõi hành vi người dùng, báo cáo doanh thu và lưu lượng truy cập, tích hợp giám sát hiệu suất.    | HTTP             |

---

## 🛠 Yêu cầu của Kiến trúc

| 🏗️ **Thành phần**               | 🛠️ **Công nghệ/Sản phẩm sử dụng**                             | 🔍 **Mô tả**                                                                                                     |
|---------------------------------|--------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------|
| **FrontEnd**                    | Telerik UI for ASP.NET Core (Webform)                        | Giao diện người dùng với component UI của Telerik cho ASP.NET Core.                                             |
| **BackEnd**                     | ASP.NET Core .NET 8                                         | Phát triển logic nghiệp vụ chính cho từng microservice.                                                        |
|                                 | Entity Framework Core                                       | ORM cho CRUD đơn giản và bảo trì dễ dàng với SQL Server.                                                        |
|                                 | Dapper                                                     | Tăng hiệu suất cho các truy vấn phức tạp.                                                                       |
|                                 | JWT Authentication                                         | Xác thực và phân quyền cho các yêu cầu giữa các microservice và API Gateway.                                    |
|                                 | Memcached                                                  | Lưu trữ tạm thời dữ liệu, giảm tải truy vấn cơ sở dữ liệu.                                                      |
| **Database**                    | SQL Server trên Docker                                      | Hệ quản trị cơ sở dữ liệu chính, lưu trữ thông tin người dùng, sản phẩm, đơn hàng.                              |
| **API Gateway**                 | Ocelot                                                     | Quản lý truy cập vào các microservices và bảo mật với JWT.                                                      |
| **Giao tiếp giữa các Service**  | HTTP và RabbitMQ                                           | HTTP cho giao tiếp đồng bộ; RabbitMQ cho các tác vụ bất đồng bộ.                                               |
| **CI/CD**                       | Docker, Docker-Compose, GitHub Actions                      | Triển khai container và tự động hóa quy trình CI/CD.                                                           |
| **Triển khai**                  | Google Cloud (VPS hoặc Shared Hosting)                      | Tối ưu chi phí và có khả năng mở rộng khi cần thiết.                                                           |
| **IDE**                         | Visual Studio Code                                         | Phát triển dự án với các extension cho ASP.NET Core và Docker.                                                 |
| **Hệ điều hành**                | Ubuntu                                                     | Hệ điều hành nền tảng cho các server, đảm bảo hiệu suất cao.                                                    |
| **Monitoring & Logging**        | Prometheus & Grafana, Console Logger                        | Giám sát hiệu suất và ghi log hệ thống.                                                                        |
| **Caching**                     | Memcached                                                  | Tối ưu hóa hiệu suất và giảm thiểu số lượng truy vấn đến cơ sở dữ liệu.                                         |
| **Message Broker**              | RabbitMQ                                                   | Xử lý tác vụ bất đồng bộ như thông báo đơn hàng và cập nhật trạng thái vận chuyển.                             |
| **Search Engine**               | ElasticSearch/OpenSearch (tùy chọn)                        | Tìm kiếm nâng cao cho sản phẩm khi lượng dữ liệu tăng cao.                                                     |
| **Health Checks**               | ASP.NET Core Health Checks, Prometheus                     | Kiểm tra sức khỏe và trạng thái của từng service.                                                              |

---

## 🔗 Tổng quan Giao tiếp giữa các thành phần

| 🔄 **Thành phần**              | 🔗 **Kết nối tới**                           | 🔄 **Giao thức**              |
|--------------------------------|---------------------------------------------|-------------------------------|
| **FrontEnd**                   | API Gateway                                 | HTTP                          |
| **API Gateway**                | Các Microservices                           | HTTP                          |
| **User Service**               | SQL Server, Notification Service            | HTTP, RabbitMQ                |
| **Product Service**            | SQL Server, Memcached                       | HTTP                          |
| **Cart Service**               | SQL Server, Memcached                       | HTTP                          |
| **Order Service**              | Payment Service, Inventory Service, Shipping Service, Notification Service | HTTP, RabbitMQ |
| **Payment Service**            | SQL Server, Order Service                   | HTTP                          |
| **Inventory Service**          | SQL Server, Order Service                   | HTTP                          |
| **Shipping Service**           | Order Service, Notification Service         | HTTP, RabbitMQ                |
| **Notification Service**       | Twilio, SendGrid/Mailgun                    | RabbitMQ                      |
| **Admin Service**              | SQL Server, Analytics Service               | HTTP                          |
| **Analytics Service**          | SQL Server, Prometheus/Grafana              | HTTP                          |

---

## 🔍 Lưu đồ Giao tiếp và Kiến trúc

1. Frontend gửi yêu cầu qua API Gateway.
2. API Gateway định tuyến yêu cầu đến các Microservices (User, Product, Cart, Order, Payment, Inventory, Shipping, Notification).
3. Các microservices giao tiếp với nhau khi cần thiết bằng HTTP hoặc RabbitMQ.
4. Notification Service gửi thông báo qua email/SMS đến người dùng khi có sự kiện quan trọng.
5. Analytics Service thu thập dữ liệu và giám sát hiệu suất hệ thống với Prometheus và Grafana.

---

## ⚙️ Triển khai và Quản lý

| 🛠️ **Triển khai & Quản lý**     | 📄 **Chi tiết**                                                                                                        |
|--------------------------------|------------------------------------------------------------------------------------------------------------------------|
| **Docker & Docker-Compose**    | Quản lý toàn bộ microservices và các dependencies trong container Docker.                                              |
| **GitHub Actions**             | CI/CD tự động hóa build, test, và deploy mỗi khi có thay đổi trên GitHub repository.                                   |
| **Google Cloud (VPS/Shared)**  | Tối ưu chi phí, triển khai container và quản lý dịch vụ với khả năng mở rộng.                                          |
| **Monitoring (Prometheus/Grafana)** | Giám sát hiệu suất và tình trạng của từng microservice, thiết lập cảnh báo khi gặp sự cố.                             |
| **Logging (Console Logger)**   | Ghi log các hoạt động và lỗi của từng microservice, hỗ trợ phát hiện lỗi và debug.                                    |
