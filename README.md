# Fahrzeugverwaltung 🚗📅

Fahrzeugverwaltung is a comprehensive vehicle management system designed to streamline vehicle reservations and administrative workflows. Built with **ASP.NET 8** and **Angular**, this application ensures efficient vehicle utilization, approval processes, and user notifications.

---

## 🌟 Features

### Vehicle Reservation
- 🚗 **Reserve Vehicles**: Book a vehicle for a specific timespan.
- ⏳ **Customizable Restrictions**:
  - Minimum/maximum reservation duration.
  - Minimum/maximum amount of days in advance for reservations.
  - Prevent overlapping reservations.

### Approval Process
- ✔️ **Reservation Approval**: Admins or designated groups can accept or decline reservation requests.
- 🔔 **Email Alerts**: Receive notifications for new reservations and status changes.

### User Invitations
- 📄 **Invite New Users**:
  - Send an all-in-one **PDF document** containing the reservation code, instructions (Anleitung), QR code, and more for easy onboarding.

### Mail Settings
- ⚙️ **Customize Notifications**:
  - Change settings to receive emails only from selected organizations.

---

## ⚙️ Technologies & Tools

### Backend
- **ASP.NET 8**: Core API development framework.
- **FastEndpoints**: High-performance, RESTful endpoint creation.
- **Entity Framework Core**: Seamless database operations.
- **MassTransit**: Streamlined asynchronous messaging.
- **Swagger**: Interactive API documentation and testing.
- **QuestPDF**: Generate professional PDF documents for user invitations.

### Frontend
- **Angular**: Clean, user-friendly interface.
- **Client Code Generation**: Automatically generate Angular services for backend integration.

### Infrastructure
- **Docker**: Containerization for easy deployment and scalability.

---

## 🚀 Getting Started

### Prerequisites
- **.NET 8 SDK**
- **Node.js** (v16+)
- **Docker**

### Backend Setup

1. Navigate to the backend directory:
   ```bash
   cd .\Backend
   ```
1. Build the Docker image:
    ```bash
   docker build -t fahrzeugverwaltung-backend .
   ```

### Frontend Setup

1. Navigate to the frontend directory:
   ```bash
   cd ../Frontend
   ```

1. Build the docker image:
   ```bash
  docker build -t fahrzeugverwaltung-frontend .
   ```
