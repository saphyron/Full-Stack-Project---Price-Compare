# Test strategy

This document describes how testing is organised in the PriceRunnerClone
backend as it looks right now.

---

## 1. Goals

- Catch obvious bugs in validation and basic logic early.
- Keep tests simple enough that they still fit in a 2-week portfolio project.
- Make it easy to run:
  - locally (`dotnet test`)
  - in CI (GitHub Actions) using one script.

---

## 2. Test projects and scripts

### 2.1 Application tests

Project:

```text
PriceRunner.Application.Tests/
  PriceRunner.Application.Tests.csproj
  PriceRunner.Application.Tests/
    ProductServiceTests.cs   (Brand + User validator tests)
```

Current tests (xUnit):

- **BrandValidatorTests**
  - checks:
    - empty name returns `"Name is required."`
    - too long name returns `"Name must be at most 255 characters."`
    - valid name returns no errors.

- **UserValidatorTests**
  - checks:
    - empty username is rejected
    - too short password is rejected
    - invalid `UserRoleId` (<= 0) is rejected
    - valid combination returns no errors
    - update without password is allowed.

These are **true unit tests** against the validation layer only –
no database or HTTP involved.

---

### 2.2 Pipeline scripts

Folder:

```text
PriceRunner.Application.Tests/scripts/
  ResetDatabase.ps1
  RunAllTests.ps1
```

- `ResetDatabase.ps1`
  - currently a no-op placeholder.
  - intended place to reset or seed the `price_runner` DB in the future
    (e.g. via MySQL CLI or Docker).

- `RunAllTests.ps1`
  - orchestrates the test pipeline:
    1. calls `ResetDatabase.ps1`
    2. runs `dotnet test PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj`
  - uses `--no-build` so CI can build once and reuse.

This script is what the CI pipeline should call as the final “pipeline test”.

---

## 3. How to run tests locally

From the repository root:

### 3.1 Build

```bash
dotnet restore
dotnet build --configuration Release
```

### 3.2 Run application unit tests directly

```bash
dotnet test PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj   --configuration Release
```

On Windows PowerShell:

```powershell
dotnet test PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj `
  --configuration Release
```

### 3.3 Run full pipeline script

On Windows PowerShell:

```powershell
.\PriceRunner.Application.Tests\scripts\RunAllTests.ps1
```

On Linux/macOS (PowerShell Core):

```bash
pwsh ./PriceRunner.Application.Tests/scripts/RunAllTests.ps1
```

---

## 4. Planned / possible future tests

The current scope is intentionally small. If more time is available,
this is the recommended order to expand the suite:

1. **Service tests**
   - Use a test MySQL database (or Docker) and test e.g. `ProductService`
     against real data.
   - Seed via SQL script or `ResetDatabase.ps1`.

2. **Endpoint integration tests**
   - Add a separate test project (e.g. `PriceRunner.Api.Tests`).
   - Host the Minimal API in memory (`WebApplicationFactory`) and call:
     - `/api/products`
     - `/api/shops`
     - `/api/data/products-flat`
   - Verify HTTP status codes and payload shapes.

3. **Smoke tests in CI**
   - Extend `RunAllTests.ps1` to:
     - run the API (`dotnet run`) in the background.
     - call a few endpoints with `curl` or PowerShell `Invoke-WebRequest`.
   - Used only in CI, not during normal local development.

---

## 5. Testing in CI (GitHub Actions)

The recommended CI flow (in yaml form):

```yaml
- name: Restore
  run: dotnet restore

- name: Build
  run: dotnet build --configuration Release --no-restore

- name: Unit tests
  run: dotnet test PriceRunner.Application.Tests/PriceRunner.Application.Tests.csproj --configuration Release --no-build

- name: Pipeline tests
  shell: pwsh
  run: ./PriceRunner.Application.Tests/scripts/RunAllTests.ps1
```

This matches the separation:

1. build
2. unit tests
3. “pipeline” script that you can grow over time.
