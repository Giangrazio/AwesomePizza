# Awesome Pizza Project

Welcome to the Awesome Pizza Project! This project is a web application for managing pizza orders, consisting of a backend built with .NET 8 and a frontend built with React.

## Project Structure

The project is organized into the following directories:

- **backend/**: Contains the .NET 8 backend code.
- **frontend/**: Contains the React frontend code.
- **.gitignore**: Specifies files and directories to be ignored by Git.
- **docker-compose.yml**: Docker Compose configuration for running the application in containers.
- **package.json**: Contains metadata and dependencies for the frontend.
- **README.md**: This file.

## Prerequisites

Before you begin, ensure you have met the following requirements:

- **Node.js**: Required for running the frontend. [Download Node.js](https://nodejs.org/)
- **.NET 8 SDK**: Required for running the backend. [Download .NET 8](https://dotnet.microsoft.com/download)
- **Docker Desktop**: Required for running the application using Docker. [Download Docker Desktop](https://www.docker.com/products/docker-desktop)

## Running the Application

You can run the application in two ways: locally or using Docker.

### Running Locally

1. **Backend Setup**:
   - Navigate to the `backend` directory.
   - Run the following command to start the backend:
     ```bash
     dotnet run
     ```
   - Ensure you have a local instance of SQL Server running and update the connection string in the `appsettings.json` file if necessary.

2. **Frontend Setup**:
   - Navigate to the `frontend` directory.
   - Install the dependencies:
     ```bash
     npm install
     ```
   - Start the frontend:
     ```bash
     npm start
     ```

### Running with Docker

1. Ensure Docker Desktop is running.
2. Navigate to the root directory of the project.
3. Run the following command to start the application using Docker Compose:
   ```bash
   docker-compose up --build
4. Access the application at http://localhost:3080 for the frontend and http://localhost:3002 for the backend. 


Testing 

Unit tests for the backend can be added using the xUnit framework. For the frontend, you can use Jest and React Testing Library. 
Contributing 

If you want to contribute to this project, please fork the repository and submit a pull request. We welcome any contributions that improve the project. 
License 

This project is licensed under the MIT License. See the LICENSE  file for details. 
Contact 

If you have any questions or feedback, please contact us at giangraziodelvento@gmail.com . 
