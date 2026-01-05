# Squares-API

API for detecting squares from 2D points. Built with ASP.NET Core Web API and SQLite.

## Description

The Squares API allows users to:

- Import points into a database
- Add and delete individual points
- Retrieve all points
- Identify all squares formed by points
- Count the number of squares

## Prerequisites

Before running the project, make sure you have:

- [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop) (optional, for container support)
- SQLite (bundled via Entity Framework Core)

## Launching Locally

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/SquaresAPI.git
   cd SquaresAPI
   ```

2. Restore packages:

   ```bash
   dotnet restore
   ```

3. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

4. Run the project

   ```bash
   dotnet run
   ```

5. Open Swagger in your browser to test endpoints:
   https://localhost:5284/swagger

## Running with Docker (Optional)

You can run the Squares API using Docker without using the command line:

1. Make sure **Docker Desktop** is installed and running.
2. Open the project in Visual Studio (or VS Code).
3. At the top of the IDE, select the Container (Dockerfile) run profile.
4. Click **Run** (green play button). This will build and start the container automatically.
5. Swagger opens in your browser to test endpoints. The port is chosen dynamically. You can find it in the Containers window → Ports tab. Example: https://localhost:<hostPort>/swagger/index.html

## SLI calculation

The API measures performance metrics to monitor responsiveness and reliability:

- Total Requests: Number of API calls handled.
- Successful Requests (2xx): Number of requests with status 200–299.
- Failed Requests (4xx/5xx): Number of requests with client or server errors.
- Average Response Time: Milliseconds taken per request.

This is implemented in middleware using a stopwatch to measure request duration and increment counters. Metrics are logged every 10 requests, helping evaluate SLI (Service Level Indicators) for the API.

Example log:

--- SLI Metrics ---
Total Requests: 10
Successful Requests (2xx): 9
Failed Requests (4xx/5xx): 1
Average Response Time: 259 ms

---

## Notes

- Squares are **computed dynamically** from the points in the database.
- Negative coordinates are supported.
- API supports **versioning**, e.g., `/api/v1/Points`.
- Response times are optimized using hashing in the square detection algorithm.
