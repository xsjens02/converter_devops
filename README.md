## **Converter_DevOps**

![Image](https://github.com/user-attachments/assets/1dc00a1d-61af-454a-bb73-53d5e48bd639)

[Click here to visit the running program](http://79.76.48.213:3000/)

This is a school exam project focused on DevOps and CI/CD pipelines.

The CI/CD pipeline is structured into three main stages:


### 1️⃣ **----- Code Integration & Quality Assurance -----**

- Automated code integration to ensure stability and correctness.
    
- Unit tests, code coverage, and mutation testing.
    
- Static code analysis using SonarQube.


### 2️⃣ **----- Automated Deployment to Staging -----**

- The full application is deployed to a staging server.
    
- Database migrations are handled using Flyway, ensuring version control and automatic setup of required tables.


### 3️⃣ **----- Testing & Performance Assurance -----**

- End-to-end testing using TestCafe.
    
- Load test with K6 to ensure performance.
  
- Spike test with K6 to ensure performance under stress.

### 🛠️ **----- Tech Stack -----**

- **Backend:** (ASP.NET) REST API 

  - **Converter:** (C# Library) that handles unit conversion logic.

  - **Monitoring:** (C# library) for centralized logging (Seq) and distributed tracing (Zipkin).

  - **FeatureToogle:** (C# Library) for feature flag management (FeatureHub).

- **Database:** MariaDB

- **Frontend:** React Client

### 📌 **----- Features -----**

This project consist of two main feature:

- **Converter** – Performs various unit-based calculations.
    
- **Memory** – Displays a history of previous conversions.


### ⚙️ **Converter Functionality**

The converter supports multiple operations between different measurement units, including:

- ✔️ **Convert** – Converts a value from one unit to another.

- ✔️ **Add** – Adds two values with potentially different units and returns the total in a target unit.

- ✔️ **Subtract** – Calculates the absolute difference between two values in a desired unit.

- ✔️ **Scale** – Scales a value by a specified factor and converts it to a target unit.

- ✔️ **Difference** – Returns the absolute difference between two units in a target unit.

- ✔️ **Percentage** – Calculates what percentage one unit is of another.
