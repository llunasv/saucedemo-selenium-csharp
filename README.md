# SauceDemo – Selenium UI Tests (C#)

Automated UI tests for [saucedemo.com](https://www.saucedemo.com), a demo e-commerce site,
built with Selenium WebDriver, C# and NUnit using the Page Object Model.

## Tech stack
- C# / .NET 9
- Selenium WebDriver 4
- NUnit 4
- Chrome (driver managed automatically by Selenium Manager)

## Test coverage
| Area     | Scenario                                                     | Status |
|----------|--------------------------------------------------------------|--------|
| Login    | Valid credentials log the user in                            | ✅ Done |
| Login    | Locked-out user sees an error                                | ✅ Done |
| Login    | Empty fields show validation errors                          | ✅ Done |
| Cart     | Adding items updates the cart badge count                    | ✅ Done |
| Checkout | Missing first name, last name or postal code blocks checkout | ✅ Done |
| Checkout | Order item total equals the sum of item prices               | ✅ Done |
| Checkout | Complete purchase end-to-end                                 | ⏳ Planned |
| Sorting  | Products sorted by price (low → high) are in correct order   | ⏳ Planned |
| Logout   | Logging out returns the user to the login page               | 💡 Optional |

## Project structure
```
SauceDemo.Tests/
├── Core/    # test lifecycle, driver creation, configuration
├── Pages/   # Page Objects – one class per screen
├── Data/    # test users and other test data
└── Tests/   # test classes
```

## Design decisions
- **Page Object Model** – tests describe *what* the user does; page classes hold
  locators and *how* to do it, so UI changes are fixed in one place.
- **Explicit waits only** – all element access goes through `BasePage`, which waits
  for elements to be visible. No `Thread.Sleep`, no implicit waits.
- **Fresh browser per test** – tests are independent and can run in any order.
- **Environment-based config** – URL and headless mode can be changed without code changes.

## Design patterns
- **Page Object Model** – one class per screen; tests never touch locators.
- **Factory** – `DriverFactory` creates and configures the browser in one place.
- **Base classes** – `BaseTest` owns the browser lifecycle, `BasePage` owns waiting
  and element interaction, so every test and page gets them for free.
- **Builder** – `CustomerBuilder` creates a valid customer by default; each test
  changes only the field it cares about (`WithoutPostalCode()`).
- **Data-driven tests** – NUnit `[TestCase]` and `[TestCaseSource]` run one test
  with multiple input sets.

## Getting started

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Google Chrome

### Run the tests
```bash
git clone https://github.com/llunasv/saucedemo-selenium-csharp.git
cd saucedemo-selenium-csharp
dotnet test
```

### Configuration
| Variable   | Default                      | Description              |
|------------|------------------------------|--------------------------|
| `HEADLESS` | `false`                      | Run Chrome without a UI  |
| `BASE_URL` | `https://www.saucedemo.com/` | Site under test          |

```powershell
$env:HEADLESS="true"; dotnet test
```

Run a single test class:
```bash
dotnet test --filter "FullyQualifiedName~LoginTests"
```

## Roadmap
- [ ] CI – run tests headless on every pull request with GitHub Actions
- [ ] Fluent Page Objects – actions return the next page (`Login()` → `InventoryPage`)
- [ ] Page load verification – each page confirms it loaded before use
- [ ] Cross-browser – choose Chrome / Firefox / Edge with a `BROWSER` variable
