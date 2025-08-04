# MockAPI Studio

**MockAPI Studio** is a modern **mock API server** with a **built-in web management panel**.  
Easily create, edit, and delete endpoints in real time, view logs, track request statistics, simulate delayed responses, and export/import configurations.  
Perfect for **frontend developers** to test apps, practice API integration, or simulate backend behavior without coding a real backend.

---

## 🌟 What's New in v2.1
- **Response Delay Simulation** – simulate slow network responses
- **Endpoint Filtering** – quickly search endpoints in the UI
- **Export & Import Endpoints** – backup and share endpoint configurations

---

## ✨ Features
- Full web-based management panel (no file editing required)
- Support for multiple HTTP methods (`GET`, `POST`, `PUT`, `DELETE`)
- Logs and statistics dashboard
- Instant endpoint changes without restarting the server
- Persistent configuration stored in `endpoints.json`
- Ready-to-run using **.NET 8**

---

## ⚡ Quick Start

### Requirements
- Install [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run Application
```bash
git clone https://github.com/NikCraftsApps/mockapi-studio.git
cd mockapi-studio
dotnet restore
dotnet run
````

### Open in Browser:

* **Web Panel:** [http://localhost:5000/panel/index.html](http://localhost:5000/panel/index.html)
* **Swagger UI:** [http://localhost:5000/swagger](http://localhost:5000/swagger)
* **Logs:** [http://localhost:5000/panel/logs.html](http://localhost:5000/panel/logs.html)
* **Stats:** [http://localhost:5000/stats](http://localhost:5000/stats)

---

## 🎓 Learn by Doing (Updated for v2.1)

Even if you're **new to APIs**, MockAPI Studio is beginner-friendly:  
*(previously called “Learn APIs by Doing (Beginner-Friendly)” in v2.0)*

### What's New in This Guide
- Added **response delay simulation** example
- Added **export/import configuration** step

### Steps

1. **Add your first endpoint** from the web panel (like before).
2. **Test it** using a browser or `fetch()` in JavaScript.
3. **Simulate slow APIs** using the new **Delay (ms)** field to test frontend loading states.
4. **Export and import endpoints** directly from the UI to quickly move or back up configurations.

---

### Example Endpoints (Updated)

```json
[
  {
    "Id": "d7f0a2ec-4ad7-4d98-8f6e-09f11f271b5b",
    "Route": "/hello",
    "Method": "GET",
    "Response": "{ \"message\": \"Hello from MockAPI Studio\" }",
    "StatusCode": 200,
    "DelayMs": 0,
    "CallCount": 0
  },
  {
    "Id": "bb34e513-23a0-4a98-bb8c-58242273f11c",
    "Route": "/user",
    "Method": "POST",
    "Response": "{ \"status\": \"created\" }",
    "StatusCode": 201,
    "DelayMs": 500,
    "CallCount": 0
  }
]
```
---

## 🛣 Roadmap

* Reset configuration to default endpoints
* Pretty JSON response editor
* Endpoint categorization

---

## 🤝 Contributing

1. Fork this repository
2. Create a branch: `git checkout -b feature-name`
3. Commit your changes: `git commit -m "Add new feature"`
4. Push to branch: `git push origin feature-name`
5. Open a Pull Request

We welcome ideas, bug reports & documentation improvements.

---

## 📄 License

MIT – free to use, modify, and distribute.

---

## 👤 Author

**Nikodem\_G**

---
**If you find this project helpful, please give it a ⭐ on GitHub!**
