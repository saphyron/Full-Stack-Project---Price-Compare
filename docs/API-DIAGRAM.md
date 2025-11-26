# API overview & diagrams

This document gives a visual overview of the HTTP API in the PriceRunnerClone
backend.

The API is built as a .NET 9 Minimal API with Dapper + MySQL.

---

## 1. High-level request flow

```mermaid
flowchart LR
    Client[Frontend client or API consumer]
        --> API[Minimal API endpoints]

    API --> App[Application services]
    App --> Db[(MySQL price_runner)]
```

- **Client** – future React frontend, tools, etc.
- **API** – endpoint groups under `/api/*` in `src/API/Endpoints`.
- **Application services** – Dapper-based services in `src/Application/Services`.
- **Database** – MySQL database `price_runner`.

---

## 2. Endpoint groups

The main route groups and their responsibility:

```mermaid
flowchart TB
    subgraph Auth
        A1["POST /api/auth/login"]
    end

    subgraph Products
        P1["GET /api/products"]
        P2["GET /api/products/{id}"]
        P3["POST /api/products"]
        P4["PUT /api/products/{id}"]
        P5["DELETE /api/products/{id}"]
        P6["GET /api/products/by-shop/{shopId}"]
        P7["GET /api/products/search"]
        P8["GET /api/products/{id}/prices"]
        P9["GET /api/products/{id}/cheapest"]
        P10["GET /api/products/{id}/history"]
        P11["GET /api/products/all-prices"]
        P12["GET /api/products/with-brand-category"]
    end

    subgraph Shops
        S1["GET /api/shops"]
        S2["GET /api/shops/{id}"]
        S3["POST /api/shops"]
        S4["PUT /api/shops/{id}"]
        S5["DELETE /api/shops/{id}"]
        S6["GET /api/shops/{id}/products"]
        S7["GET /api/shops/{id}/prices"]
    end

    subgraph Brands
        B1["GET /api/brands"]
        B2["GET /api/brands/{id}"]
        B3["POST /api/brands"]
        B4["PUT /api/brands/{id}"]
        B5["DELETE /api/brands/{id}"]
        B6["GET /api/brands/{id}/shops"]
    end

    subgraph Categories
        C1["GET /api/categories"]
        C2["GET /api/categories/{id}"]
        C3["POST /api/categories"]
        C4["PUT /api/categories/{id}"]
        C5["DELETE /api/categories/{id}"]
    end

    subgraph Prices
        PP1["CRUD /api/product-prices"]
    end

    subgraph History
        PH1["CRUD /api/product-price-history"]
    end

    subgraph Users
        U1["GET /api/users"]
        U2["GET /api/users/{id}"]
        U3["GET /api/users/by-role/{roleId}"]
        U4["POST /api/users"]
        U5["PUT /api/users/{id}"]
        U6["DELETE /api/users/{id}"]
    end

    subgraph UserRoles
        UR1["CRUD /api/user-roles"]
    end

    subgraph Data
        D1["GET /api/data/products-flat"]
        D2["GET /api/data/price-history"]
        D3["GET /api/data/shop-stats"]
        D4["GET /api/data/brand-stats"]
        D5["GET /api/data/category-stats"]
    end
```

Notes:

- `Data` endpoints are designed for **data analysis / ML / Grafana**.
- Product-related endpoints are split into:
  - general product CRUD
  - product price CRUD
  - product price history CRUD.

---

## 3. DTOs and request models

- **Request models** (API-bound):
  - `src/API/Models/*Models.cs`
  - Example: `CreateProductRequest`, `UpdateProductRequest`, `LoginRequest`.

- **Response DTOs** (application-bound):
  - `src/Application/DTOs/*.cs`
  - Designed to match SELECT projections and to be easy to consume from frontend and ML tools.

---

## 4. Error handling

All endpoints share the same global error handling:

- `src/API/Filters/ApiExceptionFilter.cs`
- Registered in `Program.cs` via `app.UseApiExceptionFilter(app.Environment);`
- Converts unhandled exceptions into a JSON payload with:
  - `statusCode`
  - `errorCode`
  - `message`
  - optional `details`, `traceId`, `path`.

This gives a consistent error contract across all endpoints.
