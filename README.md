# Messaging Platform [Demo]

## Overview
**Messaging Platform** is a real-time messaging application that allows users to create accounts, send messages, and engage in live communication. It is built using a range of modern technologies with a focus on scalability, maintainability, and adherence to **Clean Architecture** principles and **Domain-Driven Design (DDD)**. This document provides an overview of the system, its architecture, and instructions on how to set up and run the project.

## Tech Stack

- **Backend**:
  - **C#**
  - **ASP.NET Core Web API** – RESTful APIs for managing users, messages, and communication.
  - **Entity Framework Core** – ORM for database access and management.
  - **MediatR** – Mediator pattern implementation to handle commands and queries.
  - **SignalR** – For real-time communication between users.
  - **Docker** – Containerization for easy deployment and environment management.
  - **AutoMapper** – Object mapping to simplify transformations between DTOs and domain entities.
  
- **Frontend**:
  - **Angular** – Client-side framework for building responsive and dynamic user interfaces.
  - **TypeScript** – Strongly typed language for frontend development.

- **Testing**:
  - **xUnit** – Unit testing framework.
  - **Moq** – Mocking framework for unit tests.
  - **Integration Tests** – Ensure that components interact correctly.
  
## Architecture

This project follows the **Clean Architecture** pattern, which emphasizes separation of concerns and independence of components:

1. **Domain Layer**: Contains core business logic and entities following **DDD** principles. This layer is completely isolated from any external dependencies.
   
2. **Application Layer**: Implements the application's use cases using **MediatR** for CQRS (Command Query Responsibility Segregation) and defines application service interfaces.
   
3. **Infrastructure Layer**: Contains implementations of the repository patterns, database context with **EntityFrameworkCore**, and integration with external systems like SignalR for real-time communication.
   
4. **Presentation Layer (Web API)**: ASP.NET Core Web API exposing endpoints for user registration, authentication, and messaging functionalities.

5. **Client (Frontend)**: Built using **Angular** and **TypeScript**, providing a user-friendly interface for account management and real-time messaging.

## Features

- **User Authentication**: Register and log in using secure authentication protocols.
- **Real-Time Messaging**: Chat with other users using real-time messaging powered by **SignalR**.
- **Account Management**: Create and manage user accounts.
- **Message History**: Persist chat messages and allow users to view message history.
- **Scalable Architecture**: Built with clean code principles, allowing for easy extension and maintenance.
  
## Setup Instructions

### Prerequisites

- **.NET SDK** (8.0 or later)
- **Node.js** (for Angular)
- **Docker**
- **Git**

### Backend Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/sentemon/MessagingPlatform.git
   cd MessagingPlatform
   ```


2. **Set up the database**:

   Run the following commands to run database:
   ```bash
   docker compose up --build
   ```
   
   Run the following commands to apply migrations:
   ```bash
   dotnet ef database update
   ```

3. **Run the application**:
   Start the backend using the following command:
   ```bash
   dotnet run backend/src/MessagingPlatform.Api
   ```

### Frontend Setup

1. **Navigate to the frontend directory**:
   ```bash
   cd MessagingPlatform/frontend
   ```

2. **Install dependencies**:
   ```bash
   npm install
   ```

3. **Run the Angular development server**:
   ```bash
   ng serve
   ```
   The frontend will be accessible at `http://localhost:4200`.


## Docker (soon)

You can use Docker to containerize both the backend and frontend for easier deployment.

## Key Dependencies

- **ASP.NET Core**: Provides the framework for building web APIs.
- **Entity Framework Core**: Simplifies database interactions through an ORM.
- **MediatR**: Manages request and response communication between components.
- **SignalR**: Enables real-time web functionality for the messaging feature.
- **AutoMapper**: Maps objects to simplify data transfer between layers.
- **xUnit & Moq**: Provides tools for writing unit and integration tests.

## Contributing

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Commit your changes with meaningful commit messages.
4. Push your changes to your fork and create a pull request.

## License

This project is licensed under the **MIT License**. See the LICENSE file for details.

---

Feel free to modify the sections based on the specific requirements of your project.