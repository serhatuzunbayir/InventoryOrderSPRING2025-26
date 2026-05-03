# Desktop App (Staff)

WinForms client for staff-only inventory and order management. Uses the backend REST API only.

## Features
- Staff login and registration (JWT-based)
- Inventory CRUD
- Order list and status updates
- Reports tab placeholder (not implemented)

## Run
1. Start the backend API (`Backend/API_GUIDE.md` has details).
2. Launch the desktop app.

```powershell
# From repo root
cd "Desktop App"
dotnet run
```

When prompted, enter the API base URL (default `http://localhost:5000`) and log in with a Staff account.

