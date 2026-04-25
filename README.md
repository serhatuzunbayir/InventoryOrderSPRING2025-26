# InventoryOrderSPRING2025-26
# Project Requirements

## 🛠 Functional Requirements

The system's functionality is divided based on user roles: **Anonymous Users**, **Registered Customers**, and **Staff Members**.

### 🔍 General & Guest Features
- [ ] **Search:** Users can search for items by name using partial string matching.
- [ ] **Filter & Sort:** Search results can be filtered by category and sorted by price.
- [ ] **Availability:** Real-time stock status (e.g., "In Stock", "Out of Stock") is visible to all users.
- [ ] **Access Control:** Data modification and order placement are strictly restricted to authenticated users.

### 👤 Customer Features (Web Application)
- [ ] **Account Management:** Users can create accounts, log in, and update their profiles.
- [ ] **Ordering:** Customers can place orders containing one or multiple items.
- [ ] **Order History:** Logged-in users can view active order statuses and a complete history of past purchases.
- [ ] **Tracking:** A dedicated tracking page allows customers to monitor the current shipping stage of their orders.

### 💼 Staff Features (Desktop Application)
- [ ] **Inventory Management:** Staff can add, remove, and update item details, including pricing.
- [ ] **Stock Control:** Access to exact numerical stock levels for all items in the database.
- [ ] **Shipping Management:** Supports a hybrid update system (both automatic updates and manual overrides).
- [ ] **Reporting:** Generation of statistical reports (sales trends, inventory records) for administrative review.

---

## 🏗 Non-Functional Requirements

### 💻 Technical Stack & Architecture
| Requirement | Specification |
| :--- | :--- |
| **Language** | C# |
| **Framework** | .NET Framework |
| **Architecture** | Model-View-Controller (MVC) Pattern |
| **Database** | SQLite (Primary RDBMS) |
| **Localization** | All User Interfaces must be in **English** |

### 📂 System Structure
The project is divided into three distinct components to ensure modularity:
1. **Shared Backend:** A core library containing data logic and database access layers.
2. **Web Application:** The customer-facing interface for browsing and ordering.
3. **Desktop Application:** The internal tool for staff to manage inventory and logistics.

### 🛡 Reliability & Data Integrity
* **Automated Backups:** The system performs a full backup of the SQLite database daily at **12:00 AM**.
* **Concurrency Control:** To prevent over-selling, stock levels are decremented immediately and atomically upon successful order placement.
