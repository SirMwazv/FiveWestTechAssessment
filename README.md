# FiveWestTechAssessment
## Instructions to Run the USDT/ZAR Orderbook Application
This application consists of two parts:

Backend (API): The backend API built with ASP.NET Core, which connects to the VALR exchange to track the USDT/ZAR orderbook and provides a price based on the requested USDT quantity.
Frontend (React): The frontend React app that interacts with the backend API to display the USDT/ZAR price.

### Prerequisites
Before you begin, ensure you have the following installed on your machine:

Node.js (for the React frontend): Download Node.js
.NET 8.0 SDK (for the backend API): Download .NET SDK
Docker (for running the app in containers): Install Docker
Git (for cloning the repository): Install Git

#### 1. Run the Application Locally Using IDEs
Backend (ASP.NET Core API)
Clone the repository: Open your terminal or command prompt and run:


git clone https://github.com/SirMwazv/FiveWestTechAssessment.git
cd <repository-directory>/OrderbookAPI
Install dependencies: Ensure you have the .NET 8.0 SDK installed, then restore the dependencies:

dotnet restore
Run the API: Run the backend API using Visual Studio or Visual Studio Code:

If using Visual Studio:
Open the OrderbookAPI.sln solution file.
Set the OrderbookAPI as the startup project.
Press F5 to run the application in development mode.
If using Visual Studio Code:
Open the OrderbookAPI folder.
Run the following command in the terminal:

dotnet run
Test the API: Once the API is running, it will be available at http://localhost:5002. You can test the API in your browser or with a tool like Postman or curl:


curl http://localhost:5002/api/price?usdtQuantity=10
Frontend (React Application)
Navigate to the frontend directory:


cd <repository-directory>/usdt-price-app
Install dependencies: Run the following command to install the required packages:


npm install
Start the React application: Once the dependencies are installed, run:


npm start
Access the frontend: The React app will be running at http://localhost:3000. Open it in your browser and test the interaction with the backend API.

### 2. Run the Application Using Docker
 If you prefer to run both the backend and frontend using Docker, follow these steps.

Step 1: Clone the Repository
First, clone the repository:


git clone https://github.com/SirMwazv/FiveWestTechAssessment.git
cd <repository-directory>
Step 2: Build and Run Using Docker
Make sure Docker Desktop is running on your machine, then follow these steps:

Build and start the Docker containers:


docker-compose up --build
This will build both the backend and frontend images and start the containers.

Access the services:

Backend API: The API will be accessible at http://localhost:5002. You can test it using Postman or curl:


curl http://localhost:5002/api/price?usdtQuantity=10
Frontend: The frontend React app will be accessible at http://localhost:3000.

Step 3: Shut Down the Docker Containers
To stop the running containers, press Ctrl + C in the terminal where docker-compose up was run, then execute:


docker-compose down
This will stop and remove the containers.