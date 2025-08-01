# OpenAPI Mock Server v2.0

**OpenAPI Mock Server** is an open-source **mock backend** built with **C#** and **.NET 8**.  
With the new **web-based panel**, you can **create, edit, and delete endpoints** directly from your browser – **no manual file editing required**!  
Perfect for **frontend developers** who want to **practice API calls**, learn HTTP methods, or showcase their apps without setting up a real backend.

---

## 🌟 What's New in v2.0
- **Web Management Panel** – fully featured UI to add, edit, and remove endpoints  
- **Instant Changes** – no server restart required  
- **Logs Page** – monitor all requests in real time  
- **Statistics Dashboard** – view and clear call counters  
- **Unique Endpoint IDs** – safer editing and deletion (no duplicates)  

---

## ✨ Features (All Versions)
- Multiple HTTP methods supported (`GET`, `POST`, `PUT`, `DELETE`)  
- Built-in `/stats` endpoint to track request counts  
- Interactive **Swagger UI** for endpoint testing  
- Persistent endpoint storage in `endpoints.json`  
- Lightweight & ready-to-run using `.NET 8`

---

## ⚡ Quick Start

### Requirements
- Install [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run Application
```bash
git clone https://github.com/NikCraftsApps/openapi-mock-server.git
cd openapi-mock-server
dotnet restore
dotnet run
````

### Open in Browser:

* **Web Panel (NEW):** [http://localhost:5000/panel/index.html](http://localhost:5000/panel/index.html)
* **Swagger UI:** [http://localhost:5000/swagger](http://localhost:5000/swagger)
* **Logs (NEW):** [http://localhost:5000/panel/logs.html](http://localhost:5000/panel/logs.html)
* **Stats:** [http://localhost:5000/stats](http://localhost:5000/stats)

---

## 🎓 Learn APIs by Doing (Beginner-Friendly)

Even if you’ve **never worked with APIs**, this tool is made for you.

### 1. Add Your First Endpoint

Open the [Web Panel](http://localhost:5000/panel/index.html) and fill in:

* **Route:** `/hello`
* **Method:** `GET`
* **Response key:** `message`
* **Response value:** `Hello from mock server`
* **Status code:** `200`

Click **Save** → your endpoint is ready!

---

### 2. Test Your Endpoint

In your browser console or JavaScript file:

```javascript
fetch('http://localhost:5000/hello')
  .then(res => res.json())
  .then(console.log);
```

Output:

```json
{ "message": "Hello from mock server" }
```

---

### 3. Try Other HTTP Methods

Example `POST` endpoint:

* **Route:** `/user`
* **Method:** `POST`
* **Response key:** `status`
* **Response value:** `user created`
* **Status code:** `201`

Frontend call:

```javascript
fetch('http://localhost:5000/user', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ name: "John" })
}).then(res => res.json()).then(console.log);
```

---

### 4. Monitor & Learn

* **Stats:** see how many times each endpoint was called
* **Logs:** view all requests in real time
* **Edit/Delete:** instantly update endpoints and see changes live

---

## 🔧 Example Preconfigured Endpoints

```json
[
  {
    "Id": "b5c59a2e-ec5a-4c38-9e41-c42b5f421f5b",
    "Route": "/hello",
    "Method": "GET",
    "Response": "{ \"message\": \"Hello world!\" }",
    "StatusCode": 200,
    "CallCount": 0
  },
  {
    "Id": "b3fbe31f-3572-4cb4-a2c1-9d26950d7a3f",
    "Route": "/user",
    "Method": "POST",
    "Response": "{ \"status\": \"user created\", \"userId\": 123 }",
    "StatusCode": 201,
    "CallCount": 0
  },
  {
    "Id": "7c370b9e-1de7-4cd6-8b84-0a9fc2d7e6b7",
    "Route": "/products",
    "Method": "GET",
    "Response": "{ \"products\": [{ \"id\": 1, \"name\": \"Laptop\" }, { \"id\": 2, \"name\": \"Mouse\" }] }",
    "StatusCode": 200,
    "CallCount": 0
  }
]
```

---

## 🛣 Roadmap

* **Response Delay Simulation** → test frontend loading states easily
  
---

## 🤝 Contributing

1. **Fork** this repository
2. **Create** a branch: `git checkout -b feature-name`
3. **Commit** your changes: `git commit -m "Add new feature"`
4. **Push** to branch: `git push origin feature-name`
5. **Open a Pull Request**

We welcome feature ideas, bug reports & documentation improvements.

---

## 📄 License

MIT – free to use, modify, and distribute.
See the [LICENSE](LICENSE) file for details.

---

## 👤 Author

**Nikodem\_G**

---

**If you find this project helpful, please give it a ⭐ on GitHub!**
