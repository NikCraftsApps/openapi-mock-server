# OpenAPI Mock Server

OpenAPI Mock Server is an open-source mock API server built with **C#** and **.NET 8**.  
It allows frontend developers to quickly simulate backend endpoints without writing any backend code.  
This project is perfect for testing, prototyping, learning API integration, or showcasing frontend apps without needing a real backend.

---

## ✨ Features

- **Dynamic endpoint configuration** via `endpoints.json`
- **Multiple HTTP methods** supported (GET, POST, PUT, DELETE)
- **Built-in `/stats` endpoint** to track request counts
- **Interactive Swagger UI** for exploring and testing the API
- **Simple logging middleware** for request tracking
- **Ready-to-run** with `.NET 8` (no Visual Studio required)

---

## 🗂️ Project Structure

```

openapi-mock-server/
├── src/                  # Main application source code
│   ├── Program.cs        # Application entry point
│   ├── Models/           # Endpoint models
│   ├── Services/         # Core logic for endpoints and responses
│   ├── Properties/       # Launch settings
│   ├── endpoints.json    # Example mock endpoints configuration
│   ├── appsettings.json
│   └── openapi-mock-server.csproj
├── .gitignore
├── LICENSE
└── README.md

````

---

## 🚀 Quick Start

### Requirements
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run Application
```bash
git clone https://github.com/your-username/openapi-mock-server.git
cd openapi-mock-server/src
dotnet restore
dotnet run
````

### Explore API

* **Swagger UI:** [http://localhost:5000/swagger](http://localhost:5000/swagger)
* **Health check:** [http://localhost:5000/](http://localhost:5000/)
* **Stats:** [http://localhost:5000/stats](http://localhost:5000/stats)

---

## 🔧 Configuring Endpoints

Endpoints are defined in `endpoints.json`. Example:

```json
[
  {
    "route": "/hello",
    "method": "GET",
    "response": { "message": "Hello from mock server" },
    "statusCode": 200
  }
]
```

**Steps to modify endpoints:**

1. Edit `endpoints.json`
2. Restart the server

---

## 🛣 Roadmap

* Web-based UI for editing endpoints
* Response delays and scenario simulation
* Docker container support
* OpenAPI specification import

---

## 🤝 Contributing

Contributions are welcome! You can:

* Add new features or enhance existing ones
* Improve documentation
* Report bugs and suggest improvements

**Steps:**

1. Fork this repository
2. Create a feature branch: `git checkout -b feature-name`
3. Commit your changes: `git commit -m "Add new feature"`
4. Push to your branch: `git push origin feature-name`
5. Open a Pull Request

---

## 📝 License

MIT – feel free to use, modify, and share.
See the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Nikodem\_G**

If you find this project helpful, please give it a ⭐ on GitHub!

