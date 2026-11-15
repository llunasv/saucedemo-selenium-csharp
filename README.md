# SauceDemo – Selenium UI Tests (C#)

[![UI Tests](https://github.com/llunasv/saucedemo-selenium-csharp/actions/workflows/tests.yml/badge.svg?branch=dev)](https://github.com/llunasv/saucedemo-selenium-csharp/actions/workflows/tests.yml)

Automated UI tests for [saucedemo.com](https://www.saucedemo.com), a demo e-commerce site,
built with Selenium WebDriver, C# and NUnit using the Page Object Model.

## Tech stack
- C# / .NET 9
- Selenium WebDriver 4
- NUnit 4
- Chrome (driver managed automatically by Selenium Manager)
- GitHub Actions (CI)

## Test coverage
| Area     | Scenario                                                     | Status |
|----------|--------------------------------------------------------------|--------|
| Login    | Valid credentials log the user in                            | ✅ Done |
| Login    | Locked-out user sees an error                                | ✅ Done |
| Login    | Empty fields show validation errors                          | ✅ Done |
| Cart     | Adding items updates the cart badge count                    | ✅ Done |
| Cart     | Added items appear on the cart page                          | ✅ Done |
| Cart     | Removing an item (from the product list or the cart) updates the cart | ✅ Done |
| Checkout | Missing first name, last name or postal code blocks checkout | ✅ Done |
| Checkout | Order item total equals the sum of item prices               | ✅ Done |
| Checkout | Complete purchase end-to-end shows the order confirmation    | ✅ Done |
| Checkout | Cart is empty after a completed order                        | ✅ Done |
| Sorting  | Products sorted by name (A → Z, Z → A) are in correct order  | ✅ Done |
| Sorting  | Products sorted by price (low → high, high → low) are in correct order | ✅ Done |
| Logout   | Logging out returns the user to the login page               | ✅ Done |
| Logout   | After logout, the product page can't be opened by URL        | ✅ Done |

## Bug reports
SauceDemo's intentionally broken accounts (`problem_user`, `error_user`) were tested
against `standard_user` as the reference. **8 bugs** are documented with steps to
reproduce, expected vs actual results, severity / priority and screenshots in
[docs/bug-reports.md](docs/bug-reports.md). Three of them were first caught by this
test suite; the rest were found by exploratory testing.

## Project structure
```
docs/        # bug reports and their screenshots
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
- **Stale-element safe** – waits retry when the page re-renders an element (e.g. the
  product list after sorting), and pages never cache `IWebElement`s, only locators.
- **Actions wait for their result** – `RemoveFromCart()` waits until the Remove button
  is gone before returning, so the next check never races the page's re-render.
- **Animations are waited out** – the side menu's Logout link counts as visible to
  Selenium ~50 ms into its 0.5 s slide-in while still mostly off-screen, so `Logout()`
  waits for the slide to finish before clicking.
- **Known bugs live in the tests** – the end-to-end checkout test also runs for
  `problem_user` and `error_user`, skipped with the bug ID as the reason
  (`Ignore = "BUG-004: ..."`). The suite stays green, the report lists the open bugs,
  and deleting the `Ignore` re-checks a bug once it's fixed.
- **Fresh browser per test** – tests are independent and can run in any order.
- **Environment-based config** – URL and headless mode can be changed without code changes.

## Design patterns
- **Page Object Model** – one class per screen; tests never touch locators.
- **Fluent Page Objects** – actions return the page they lead to, so tests read as a
  user journey and the compiler enforces the flow:
  `Login()` → `InventoryPage` → `OpenCart()` → `CartPage` → `Checkout()` →
  `CheckoutInformationPage` → `Continue()` → `CheckoutOverviewPage` → `Finish()` →
  `CheckoutCompletePage`.
  Failure paths are explicit (`LoginExpectingError()`, `ContinueExpectingError()`).
- **Page load verification** – every page waits for an element unique to it when created
  and fails with a clear message (`InventoryPage did not load...`) instead of a vague
  timeout several steps later.
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

## Continuous integration
Every pull request and every push to `dev` / `main` runs the full test suite on
GitHub Actions (Ubuntu, Chrome in headless mode). Test results are uploaded as a
`.trx` artifact on each run, including failed ones. The workflow can also be started
manually from the **Actions** tab.

## Roadmap
- [ ] Cross-browser – choose Chrome / Firefox / Edge with a `BROWSER` variable
- [ ] Test case design – `docs/test-cases.md` with test cases named by technique
  (equivalence partitioning, boundary values, decision table, state transitions)
  and linked to the automated tests that cover them
