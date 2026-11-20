# Test cases – SauceDemo

24 test cases for the main flows of https://www.saucedemo.com, each designed with a named
test design technique. 23 of them are covered by an automated test in this repository; 1 is
`Manual` (see [Why a case stays manual](#why-a-case-stays-manual)).

## Techniques used
| Technique | Idea | Used for |
|---|---|---|
| **EP** – equivalence partitioning | Split the inputs into classes that the system treats the same way and test one representative per class. | Login users, sort options |
| **BVA** – boundary value analysis | Defects cluster at the edges of a range, so test the edges. | Cart badge: 0, 1 and 6 (max) items |
| **Decision table** | List every combination of conditions and the expected outcome of each. | Login (empty / filled fields), checkout form |
| **State transition** | The system has states; test the actions that move it from one state to another. | Add → Remove → Add, logout |
| **E2E scenario** | One complete user journey through several pages. | Buying an order |

## How to read the tables
| Column | Meaning |
|---|---|
| **ID** | `TC-<area>-<number>`: `LOG` login, `CRT` cart, `SRT` sorting, `CHK` checkout, `LGT` logout. |
| **Title** | What is checked, in one line. |
| **Technique** | The technique that produced the case. `R4` is a rule (column) of the decision table above it. |
| **Steps** | Numbered actions. |
| **Expected** | What the user sees. Error texts are copied from the site, not paraphrased. |
| **Automated by** | The NUnit test that runs the case, or `Manual`. |

**Test data.** User `standard_user`, password `secret_sauce`, unless a case says otherwise.
Every case starts on the login page (https://www.saucedemo.com). Cart and checkout cases start
with an empty cart.

**Verified.** Automated cases are checked by their tests on every CI run. The Expected results
of the manual cases and all eight checkout combinations were checked by hand on the live site on
2026-11-20 in Chrome.

---

## Login

### Equivalence partitioning
The login form accepts a username and a password. The inputs fall into four classes by
what the site does with them:

1. valid user with the right password,
2. valid user that is locked out,
3. valid user with a wrong password,
4. a user that does not exist.

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-LOG-01 | Login with a valid user | EP – valid user | 1. Enter `standard_user` and `secret_sauce`<br>2. Click Login | The Products page opens (`/inventory.html`) | `LoginTests.Login_WithValidCredentials_OpensInventoryPage` |
| TC-LOG-02 | Login with a locked-out user | EP – locked-out user | 1. Enter `locked_out_user` and `secret_sauce`<br>2. Click Login | Error "Epic sadface: Sorry, this user has been locked out."; stays on the login page | `LoginTests.Login_WithLockedOutUser_ShowsLockedOutError` |
| TC-LOG-03 | Login with a wrong password | EP – wrong password | 1. Enter `standard_user` and `wrong_password`<br>2. Click Login | Error "Epic sadface: Username and password do not match any user in this service"; stays on the login page | `LoginTests.Login_WithInvalidCredentials_ShowsMismatchError("standard_user", "wrong_password")` |
| TC-LOG-04 | Login with a user that does not exist | EP – unknown user | 1. Enter `no_such_user` and `secret_sauce`<br>2. Click Login | Error "Epic sadface: Username and password do not match any user in this service"; stays on the login page | `LoginTests.Login_WithInvalidCredentials_ShowsMismatchError("no_such_user", "secret_sauce")` |

### Decision table: empty / filled fields
| Condition / action | R1 | R2 | R3 | R4 |
|---|---|---|---|---|
| Username filled | yes | no | yes | no |
| Password filled | yes | yes | no | no |
| **Outcome** | credentials are checked | "Username is required" | "Password is required" | "Username is required" |

R1 is the same as the four classes above (TC-LOG-01…04), so it has no case of its own. When
both fields are empty the site reports the username first.

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-LOG-05 | Login with an empty username | Decision table (R2) | 1. Leave Username empty, enter `secret_sauce` as password<br>2. Click Login | Error "Epic sadface: Username is required" | `LoginTests.Login_WithMissingCredentials_ShowsRequiredFieldError("", "secret_sauce")` |
| TC-LOG-06 | Login with an empty password | Decision table (R3) | 1. Enter `standard_user`, leave Password empty<br>2. Click Login | Error "Epic sadface: Password is required" | `LoginTests.Login_WithMissingCredentials_ShowsRequiredFieldError("standard_user", "")` |
| TC-LOG-07 | Login with both fields empty | Decision table (R4) | 1. Leave both fields empty<br>2. Click Login | Error "Epic sadface: Username is required" | `LoginTests.Login_WithMissingCredentials_ShowsRequiredFieldError("", "")` |

---

## Cart

### Boundary values: cart badge
The Products page has 6 products, so the cart holds 0 to 6 items. The badge on the cart icon
is hidden at 0, appears at 1 and is at its maximum at 6.

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-CRT-01 | No badge on an empty cart | BVA – lower boundary (0) | 1. Log in | The cart icon has no badge | `CartTests.Cart_WhenEmpty_ShowsNoBadge` |
| TC-CRT-02 | Badge appears with the first item | BVA – just above the lower boundary (1) | 1. Log in<br>2. Click Add to cart on Sauce Labs Backpack | Badge shows `1` | `CartTests.AddToCart_OneItem_ShowsBadgeCountOne` |
| TC-CRT-03 | Badge with all 6 products | BVA – upper boundary (6) | 1. Log in<br>2. Click Add to cart on every product, one by one | After each click the badge goes up by one and ends at `6`; no Add to cart button is left | Manual |
| TC-CRT-04 | Badge disappears when the last item is removed | BVA – back to the lower boundary (1 → 0) | 1. Log in<br>2. Add Sauce Labs Backpack<br>3. Click Remove on the same product | The badge is gone | `CartTests.RemoveFromCart_OnInventoryPage_HidesBadge` |

### State transition: a product's button
A product on the Products page is in one of two states: **not in cart** (button "Add to
cart", no badge contribution) or **in cart** (button "Remove", counted in the badge).

| From → to | Action | Result |
|---|---|---|
| not in cart → in cart | click Add to cart | button becomes Remove, badge `1` |
| in cart → not in cart | click Remove | button becomes Add to cart, badge gone |
| not in cart → in cart (again) | click Add to cart | same as the first transition |

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-CRT-05 | Add, remove and add the same product again | State transition | 1. Log in<br>2. Add Sauce Labs Backpack<br>3. Click Remove<br>4. Click Add to cart again | After step 2: button "Remove", badge `1`. After step 3: button "Add to cart", no badge. After step 4: button "Remove", badge `1` | `CartTests.AddToCart_AfterRemovingSameItem_ShowsBadgeCountOne` (checks the badge after each Remove / Add; the button labels are checked by hand) |

---

## Sorting

### Equivalence partitioning: sort options
The dropdown has four options, and each one is its own class: the site must order the list
differently for each.

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-SRT-01 | Sort by name A → Z | EP – `az` | 1. Log in<br>2. Select "Name (Z to A)", then "Name (A to Z)" | Product names are in ascending alphabetical order | `SortingTests.SortByName_AToZ_ProductsAreOrderedAlphabetically` |
| TC-SRT-02 | Sort by name Z → A | EP – `za` | 1. Log in<br>2. Select "Name (Z to A)" | Product names are in descending alphabetical order | `SortingTests.SortByName_ZToA_ProductsAreOrderedReverseAlphabetically` |
| TC-SRT-03 | Sort by price low → high | EP – `lohi` | 1. Log in<br>2. Select "Price (low to high)" | Prices are in ascending order | `SortingTests.SortByPrice_LowToHigh_ProductsAreOrderedAscending` |
| TC-SRT-04 | Sort by price high → low | EP – `hilo` | 1. Log in<br>2. Select "Price (high to low)" | Prices are in descending order | `SortingTests.SortByPrice_HighToLow_ProductsAreOrderedDescending` |

TC-SRT-01 selects Z → A first on purpose: A → Z is the default order, so selecting it alone
would pass even if sorting did nothing.

---

## Checkout

### Decision table: the three form fields
The checkout form has First Name, Last Name and Postal Code. Each is filled or empty, which
gives 2 × 2 × 2 = 8 combinations. The site checks the fields in order and reports the first
empty one, so with several empty fields only one error is shown.

| Condition / action | R1 | R2 | R3 | R4 | R5 | R6 | R7 | R8 |
|---|---|---|---|---|---|---|---|---|
| First Name filled | yes | no | yes | yes | no | no | yes | no |
| Last Name filled | yes | yes | no | yes | no | yes | no | no |
| Postal Code filled | yes | yes | yes | no | yes | no | no | no |
| **Outcome** | Overview page | "First Name is required" | "Last Name is required" | "Postal Code is required" | "First Name is required" | "First Name is required" | "Last Name is required" | "First Name is required" |
| **Case** | TC-CHK-01 | TC-CHK-02 | TC-CHK-03 | TC-CHK-04 | – | – | – | – |

R1–R4 (everything filled, and each field missing alone) are written out below. R5–R8 have two
or three empty fields; they add no new outcome because the first empty field wins, so they are
not written out as cases.

All cases below: log in, add Sauce Labs Backpack, open the cart, click Checkout.

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-CHK-01 | Checkout with all fields filled | Decision table (R1) | 1. Enter First Name `John`, Last Name `Doe`, Postal Code `12345`<br>2. Click Continue | The Overview page opens (`/checkout-step-two.html`) | Indirectly: `CheckoutTests.Checkout_WithValidInformation_ShowsOrderConfirmation("standard_user")` cannot pass without it |
| TC-CHK-02 | Checkout with empty First Name | Decision table (R2) | 1. Enter Last Name and Postal Code, leave First Name empty<br>2. Click Continue | Error "Error: First Name is required", stays on the page | `CheckoutTests.Checkout_WithMissingField_ShowsRequiredFieldError("missing first name")` |
| TC-CHK-03 | Checkout with empty Last Name | Decision table (R3) | 1. Enter First Name and Postal Code, leave Last Name empty<br>2. Click Continue | Error "Error: Last Name is required", stays on the page | `CheckoutTests.Checkout_WithMissingField_ShowsRequiredFieldError("missing last name")` |
| TC-CHK-04 | Checkout with empty Postal Code | Decision table (R4) | 1. Enter First and Last Name, leave Postal Code empty<br>2. Click Continue | Error "Error: Postal Code is required", stays on the page | `CheckoutTests.Checkout_WithMissingField_ShowsRequiredFieldError("missing postal code")` |

### End-to-end scenarios
Both products used below: Sauce Labs Backpack ($29.99) and Sauce Labs Bike Light ($9.99).

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-CHK-05 | Buy two products | E2E scenario | 1. Log in, add Backpack and Bike Light<br>2. Open the cart, click Checkout<br>3. Enter a valid First Name, Last Name and Postal Code, click Continue<br>4. Click Finish | The confirmation page shows "Thank you for your order!" | `CheckoutTests.Checkout_WithValidInformation_ShowsOrderConfirmation("standard_user")` |
| TC-CHK-06 | Item total equals the sum of the item prices | E2E scenario | 1. Log in, add Backpack and Bike Light<br>2. Open the cart, click Checkout<br>3. Enter valid information, click Continue | On the Overview page "Item total" is $39.98, the sum of the two item prices | `CheckoutTests.Checkout_Overview_ItemTotalEqualsSumOfItemPrices` |
| TC-CHK-07 | Cart is empty after the order | E2E scenario | 1. Complete the purchase as in TC-CHK-05<br>2. Click Back Home | The Products page opens and the cart icon has no badge | `CheckoutTests.Checkout_AfterOrderCompleted_CartIsEmpty` |

---

## Logout

### State transition
The user is in one of two states: **logged in** or **logged out**. The interesting question is
whether "logged out" is enforced by the site or only by the missing menu: after logging out, can
a protected page still be opened by typing its URL?

| From → to | Action | Result |
|---|---|---|
| logged in → logged out | menu → Logout | login page |
| logged out → (stays logged out) | open `/inventory.html` by URL | login page with an error, no products |

| ID | Title | Technique | Steps | Expected | Automated by |
|---|---|---|---|---|---|
| TC-LGT-01 | Protected page is closed after logout | State transition | 1. Log in<br>2. Open the menu and click Logout<br>3. Open https://www.saucedemo.com/inventory.html in the same tab | Step 2: the login page opens. Step 3: the login page with the error "Epic sadface: You can only access '/inventory.html' when you are logged in." | `LogoutTests.Logout_FromMenu_OpensLoginPage` (step 2), `LogoutTests.OpenInventory_AfterLogout_ShowsLoginRequiredError` (steps 2–3) |

---

## Why a case stays manual
Only **TC-CRT-03** (badge with all 6 products) has no automated test. The 6-product maximum is a
fixed catalogue, so a regression there is unlikely, and the case needs six clicks for one
assertion. It was run by hand on 2026-11-20 and is the first one to automate if the catalogue
ever grows or becomes configurable.

TC-LOG-03, TC-LOG-04 and TC-CRT-05 were manual in the first version of this document and are now
automated. Rules R5–R8 of the checkout table are not cases at all, see the note under the table.

## Automated tests without a case of their own
These tests exist but are not mapped to a case, because they repeat a boundary or state that is
already covered:

- `SmokeTest.LoginPage_Opens`
- `CartTests.AddToCart_TwoItems_ShowsBadgeCountTwo`
- `CartTests.OpenCart_AfterAddingTwoItems_ShowsBothItems`
- `CartTests.RemoveFromCart_OnCartPage_KeepsOtherItems`
- `CheckoutTests.Checkout_WithValidInformation_ShowsOrderConfirmation("problem_user")` and
  `("error_user")`: skipped, see [bug-reports.md](bug-reports.md) (BUG-004, BUG-002)
