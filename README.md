# 🧱 Clean Architecture Blog – ASP.NET Core MVC (.NET 8)

A simple but well-structured **blog application** built with **ASP.NET Core MVC (.NET 8)** — designed with **SOLID principles**, **OOP fundamentals**, and layered architecture to allow easy scaling for future business logic.

While it's currently a blog platform, the project is structured in a way that it can be extended for **any type of domain-driven application** (e.g. e-commerce, task manager, CMS, etc.).

---

## 📦 Project Architecture

This project follows a **layered clean architecture** with clear separation of concerns:
Solution/
├── Core/ # Entities, Enums, Interfaces (domain contracts)
├── Domain/ # Domain logic and aggregates
├── Application/ # DTOs, UseCases, Services, Validators
├── Infrastructure/ # External services, Email, FileSystem, etc.
├── Data/ # DBContext, Entity configs, Seeders
├── DataAccess/ # Repositories, EFCore queries
├── Logging/ # Serilog, Log abstraction or adapters
├── Web/ # ASP.NET MVC project (controllers, views)
└── Tests/ # (Optional) Unit & integration tests

---

## ✅ Features

- 📰 Blog system with posts, categories, and tags
- 🧠 Fully layered with **OOP** and **SOLID** principles
- 🧱 Uses **Clean Architecture** & **Separation of Concerns**
- 📦 Easily pluggable into any business logic (blog is just a sample)
- 💾 EF Core 8 for database access
- 🧩 Interfaces + Dependency Injection for flexibility
- 📜 Logging layer ready (e.g., Serilog)
- 📂 Follows **CQRS-like** flow between Application and Domain layers

---

## 🧠 Design Principles

- **S** – Single Responsibility: Each class and layer has one job
- **O** – Open/Closed: System is open to extension, closed to modification
- **L** – Liskov Substitution: Interfaces are replaceable by implementations
- **I** – Interface Segregation: Interface boundaries are clean and focused
- **D** – Dependency Inversion: High-level modules don’t depend on low-level implementations

---

## 🚀 Getting Started

1. Clone the project

```bash
git clone https://github.com/your-username/clean-architecture-blog.git
```
2. Open in Visual Studio 2022+ or Rider

3. Set Web project as startup

4. Apply migrations

```bash
cd Data
dotnet ef database update
```
5. Run the project and visit:
```
https://localhost:5001/
```
📂 Key Layers Explained
🔸 Core
Contains base entities, interfaces, and shared enums

No dependencies on other layers

🔸 Domain
Contains the main business rules and domain models

🔸 Application
Use Cases, DTOs, service interfaces, mapping profiles

Validation and flow logic happens here

🔸 Infrastructure
External concerns like emailing, file storage, etc.

🔸 Data & DataAccess
EF Core DbContext

Repository pattern

Query/Command separation if needed

🔸 Logging
Logging abstraction, ready to be swapped with any logger (e.g., Serilog, NLog)

🔸 Web (MVC)
The actual ASP.NET Core MVC project

Razor views, controllers, models, etc.

🔍 Example Use Cases
✅ Simple blog or CMS

✅ Extendable to product catalogs, user portals, dashboards

✅ Useful template for any business logic with domain separation

🧪 Future Improvements
Add Authentication & Authorization (JWT / Identity)

RESTful API layer for frontend/mobile use

Unit tests with xUnit & Moq

CQRS with MediatR (optional)

🙋‍♂️ Author
Made with care by @fuadsadiqov
For those who prefer architecture over chaos. 😎
