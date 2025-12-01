## Inspiration

This project is inspired by price comparison platforms such as PriceRunner.  
All code, database structures, and UI components are implemented from scratch
for educational and portfolio purposes only.  
We are not affiliated with, endorsed by, or connected to PriceRunner in any way.


# PriceRunnerClone backend (Frontend Comming Later)

A small .NET 9 Minimal API backend that mimics some core ideas from
price comparison sites (like PriceRunner):

- Products, shops, brands, categories
- Current product prices and historical price data
- Data endpoints optimised for analysis, ML and dashboards

This repository currently contains the **backend + tests**.
A React frontend and web crawler are planned but not included yet.

---

## Table of contents

- [Features](#features)
- [Tech stack](#tech-stack)
- [Architecture overview](#architecture-overview)
- [Project structure](#project-structure)
- [Getting started](#getting-started)
- [API overview](#api-overview)
- [Testing](#testing)
- [Known limitations](#known-limitations)
- [Authors](#authors)
- [License](#license)

---

## Features

- CRUD endpoints for:
  - brands, categories
  - shops
  - products
  - product prices
  - product price history
  - users and user roles
- Simple login endpoint:
  - `POST /api/auth/login`
  - password stored as SHA256 hash (demo only, not production security).
- Data endpoints aimed at ML / Grafana:
  - flat product data with brand, category, shop, current price
  - price history time series
  - simple shop, brand and category statistics.
- Centralised error handling:
  - JSON errors with `statusCode`, `errorCode`, `message`, etc.
- Dapper-based data access:
  - clear SQL, easy to tune for queries and analytics.

---

## Tech stack

- **Language:** C# 13 (via .NET 9)
- **Web:** .NET 9 Minimal API
- **Database:** MySQL (`price_runner` database)
- **Data access:** Dapper + `MySqlConnector`
- **Testing:** xUnit, dotnet test
- **Config:** `appsettings.json` + environment variables

---

## Architecture overview

The backend is split into three main layers:

- **API layer (`src/API`)**
  - Minimal APIs, routing and request/response models.
  - Global exception filter (`ApiExceptionFilter`).
  - OpenAPI/Swagger configuration.

- **Application layer (`src/Application`)**
  - DTOs, validation, Dapper-based services.
  - All business logic that talks to the database.

- **Infrastructure layer (`src/Infrastructure`)**
  - Database options and connection factory.
  - Migration script placeholders.

A future **Crawler layer** exists under `src/Crawler`, but is intentionally
not wired into the MVP yet.

For more details, see **`ARCHITECTURE.md`**.

---

## Project structure (Not Updated Yet)

Legend: 📁 Folder • 🧩 C#-Code • ⚙️ config/json/yaml • 🪪 .sln/.csproj • 🧾 Docs/Markdown • 🧪 Tests • 🐳 Docker/CI
```text
📁 PriceRunnerClone
  🪪 PriceRunnerClone.sln
  🧾 README.md
  🧾 LICENSE
  ⚙️ .env
  ⚙️ .env.app
  ⚙️ .gitignore
  📁 docs
    🧾 ARCHITECTURE.md          (lag, SOLID, diagrammer)
    🧾 ER-DIAGRAM.md            (db-modeller og relationer)
    🧾 API-DESIGN.md            (endpoints, DTO’er)
    🧾 TEST-STRATEGY.md         (hvad tester du hvor)
    🧾 PRESENTATION-NOTES.md    (hjælp til 15 min. oplæg)

  📁 backend
    🪪 PriceRunner.Api.csproj

    📁 src
      📁 Domain                   ← Forretningsmodel, ren C#
        🧩 Product.cs
        🧩 Shop.cs
        🧩 Price.cs
        🧩 PriceHistory.cs
        🧩 User.cs
        📁 Value
          🧩 Money.cs
          🧩 ProductId.cs
        📁 Interfaces
          🧩 IProductRepository.cs
          🧩 IShopRepository.cs
          🧩 IPriceService.cs

      📁 Application               ← Services, DTOs, use-cases
        📁 DTOs
          🧩 ProductDto.cs
          🧩 ProductDetailDto.cs
          🧩 ShopDto.cs
        📁 Services
          🧩 ProductService.cs
          🧩 PriceService.cs
          🧩 AuthService.cs
        📁 Mappers
          🧩 ProductMapper.cs
        📁 Validation
          🧩 ProductValidator.cs

      📁 Infrastructure            ← EF Core, SQL, repos, migrations
        📁 Data
          🧩 AppDbContext.cs
          🧩 SeedData.cs
        📁 Configurations
          🧩 ProductConfiguration.cs
          🧩 ShopConfiguration.cs
        📁 Repositories
          🧩 ProductRepository.cs
          🧩 ShopRepository.cs
        📁 Migrations              (autogenereret af EF)
          ⚙️ 20251124_InitialCreate.cs
        ⚙️ appsettings.json
        ⚙️ appsettings.Development.json

      📁 Api                       ← Web API lag
        🧩 Program.cs              (DI, pipeline, routing, Swagger)
        📁 Endpoints
          🧩 ProductEndpoint.cs
          🧩 ShopEndpoint.cs
          🧩 PriceEndpoint.cs
          🧩 AuthEndpoint.cs
        📁 Filters
          🧩 ApiExceptionFilter.cs
        📁 Models                  ← request/response-modeller
          🧩 CreateProductRequest.cs
          🧩 UpdateProductRequest.cs

      📁 Crawler (extra)
        🧩 PriceCrawlerService.cs  (IHostedService background job)
        📁 Providers
          🧩 IShopCrawler.cs            (interface)
          🧩 ExampleShopCrawler.cs      (konkret implementation)
        📁 Parsing
          🧩 HtmlPriceParser.cs

    📁 tests
      📁 PriceRunner.Domain.Tests       ← rene unit tests
        🧪 ProductTests.cs
      📁 PriceRunner.Application.Tests  ← service-lag
        🧪 ProductServiceTests.cs
      📁 PriceRunner.Api.Tests          ← simple integration tests
        🧪 ProductsEndpointTests.cs

    📁 scripts
      🧩 ResetDatabase.ps1
      🧩 RunAllTests.ps1

  📁 frontend
    🪪 pricerunner-frontend.csproj (hvis du kører ASP.NET+React template)
    📁 src
      📁 api
        🧩 httpClient.ts          (axios/fetch wrapper)
        🧩 productsApi.ts
        🧩 authApi.ts
      📁 components
        🧩 ProductCard.tsx
        🧩 PriceTag.tsx
        🧩 ShopBadge.tsx
        🧩 Layout.tsx
      📁 pages
        🧩 ProductsPage.tsx       (liste med søgning/filter)
        🧩 ProductDetailPage.tsx  (alle shops + billigste pris)
        🧩 CartPage.tsx           (watchlist/kurv)
        🧩 AdminProductsPage.tsx  (CRUD for admin)
        🧩 LoginPage.tsx
      📁 context
        🧩 CartContext.tsx
        🧩 AuthContext.tsx
      📁 hooks
        🧩 useProducts.ts
        🧩 useAuth.ts
      📁 routing
        🧩 AppRouter.tsx
      📁 styles
        ⚙️ main.css / Tailwind config
    ⚙️ vite.config.ts / package.json

  📁 monitoring
    📁 grafana
      ⚙️ grafana-datasource.yml    (SQL connection)
      ⚙️ grafana-dashboard.json    (prisstatistik mv.)

  📁 deploy
    🐳 docker-compose.yml          (api + sql + frontend + grafana)
    🐳 Dockerfile.backend
    🐳 Dockerfile.frontend
    🐳 .dockerignore
    
  📁 .github
    📁 workflows
      ⚙️ ci.yml       (byg + test + evt. docker build)

```

---

## Getting started

### 1. Prerequisites

- .NET 9 SDK installed
- MySQL server running locally (or in Docker)
- A database called `price_runner`

### 2. Create database schema

The full schema is defined in the SQL Script:

- tables: `user_roles`, `users`, `brands`, `categories`, `shops`,
  `products`, `product_prices`, `products_history`
- appropriate foreign keys between them.

You can paste that script into your MySQL client and run it against the
`price_runner` database.

The files `src/Infrastructure/Migrations/001_create_schema.sql` and
`002_seed_data.sql` are placeholders where this script and seed data can be
stored later if you want to automate it.

### 3. Configure connection string

In development, you can either:

1. Set it in `appsettings.Development.json`:

```json
"Database": {
  "ConnectionString": "Server=localhost;Port=3306;Database=price_runner;User Id=...;Password=...;"
}
```

2. Or set an environment variable:

```bash
export MYSQL_CONNECTION_STRING="Server=localhost;Port=3306;Database=price_runner;User Id=...;Password=...;"
```

The infrastructure layer (`AddInfrastructure`) will pick up the connection
string from configuration or `MYSQL_CONNECTION_STRING`.

### 4. Run the API

From the repository root:

```bash
dotnet run --project PriceRunnerClone.csproj
```

Local URLs (from `launchSettings.json`):

- HTTP:  `http://localhost:5282`
- HTTPS: `https://localhost:7103`

Swagger UI is enabled in `Development` and can be reached at:

- `https://localhost:7103/swagger`
- or `http://localhost:5282/swagger`

---

## API overview

The most important route groups are:

- `POST /api/auth/login`
- `GET/POST/PUT/DELETE /api/products`
- `GET /api/products/{id}/prices`
- `GET /api/products/{id}/cheapest`
- `GET /api/products/{id}/history`
- `GET/POST/PUT/DELETE /api/shops`
- `GET /api/shops/{id}/products`
- `GET /api/shops/{id}/prices`
- `GET/POST/PUT/DELETE /api/brands`
- `GET/POST/PUT/DELETE /api/categories`
- `GET/POST/PUT/DELETE /api/product-prices`
- `GET/POST/PUT/DELETE /api/product-price-history`
- `GET/POST/PUT/DELETE /api/users`
- `GET/POST/PUT/DELETE /api/user-roles`

Data/analytics endpoints:

- `GET /api/data/products-flat`
- `GET /api/data/price-history`
- `GET /api/data/shop-stats`
- `GET /api/data/brand-stats`
- `GET /api/data/category-stats`

For diagrams, see **`API-DIAGRAM.md`**.

---

## Testing

### Run tests locally

```bash
dotnet test PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj   --configuration Release
```

or on Windows PowerShell:

```powershell
dotnet test PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj `
  --configuration Release
```

### Run full pipeline script

```powershell
.\PriceRunner.Application.Tests\scripts\RunAllTests.ps1
```

See **`TEST-STRATEGY.md`** for details about what is currently covered and
suggestions for future tests.

---

## Known limitations

- No real authentication or authorisation:
  - passwords use SHA256 without salt
  - no JWT, roles are not enforced on endpoints.
- No automated migrations:
  - database schema is created manually via SQL script.
- Crawler layer is a placeholder:
  - `PriceCrawlerService` and related classes are not yet implemented.
- No frontend in this repository:
  - API is ready to be consumed by a React app, but that project is not ready yet.

---

## Authors

- **Nikolaj Østergaard Rasmussen** – [github.com/NikolajOR](https://github.com/NikolajOR)
- **John Grandt Markvard Høeg** – [github.com/saphyron](https://github.com/saphyron)

---

## License

The project is intended to be released under the **MIT License**.