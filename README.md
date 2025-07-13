# Late and Absence Request System - Backend API

This is the backend for the Late and Absence Request System — a secure, role-based ASP.NET Core 9.0 Web API designed to manage student absence/lateness requests,
approvals across multiple admin levels, and real-time and email notifications for timely processing.

>  Institutional names have been intentionally omitted for privacy.

## Built With

- **ASP.NET Core 9.0**
- **Entity Framework Core 9**
- **ASP.NET Core Identity**
- **SQL Server**
- **JWT Authentication**
- **SendGrid** (Email delivery)
- **SignalR** (Real-time communication)
- **Hangfire** (Recurring background jobs)
- **Autofac** (Dependency injection)
- **AutoMapper** (Model mapping)
- **Swashbuckle** (Swagger for API docs)

---

##  Roles and Access

- **Student** – Can submit request forms.
- **Secretary** – First and Second-level approval.
- **Chairperson** – Second-level approval.
- **Director** – Final approval.

The system ensures that requests move sequentially and only the appropriate roles receive and act on notifications.

---

## Features

- JWT-based authentication with Identity Core
- Request lifecycle: submit → review → approve/reject
- Email alerts to admins via SendGrid
- Real-time request updates using SignalR
- Hangfire-powered reminders for pending requests
- Swagger UI for exploring and testing API endpoints
- Clean architecture with DI, controllers, services, repositories, and mapping

## Authentication

Admin users are authenticated using JWT.  
Endpoints are protected using `[Authorize]` and `[Authorize(Roles = "...")]`.

## Email Notifications

Admins (Secretary, Chairperson, Director) receive real-time and email alerts when requests are submitted or pending for over a threshold time.  
Email service is powered by SendGrid and Hangfire.

## Author

**Paulo Xedric Lozano**  
GitHub: [@PauloXedric](https://github.com/PauloXedric)  
100% Designed, built, and maintained by me.

## Frontend

The frontend Angular client is available here:  
👉 [Frontend Repository](https://github.com/PauloXedric/LateAndAbsenceRequestSystem-frontend)
