# Online Examination Management System

A scalable, web-based platform for managing examinations and quizzes. Designed to empower instructors to create and administer courses, quizzes, and final exams, while providing students with a secure and seamless assessment experience.

Built with clean architecture principles, emphasizing **maintainability**, **scalability**, **security**, and **performance**.

---

## Features

### Instructor
- Create, edit, and delete courses
- Create and manage quizzes and final exams
- Add questions with configurable difficulty levels (Simple, Medium, Hard)
- Reuse questions across multiple exams
- Manually assign or automatically generate exams with balanced difficulty distribution
- Assign students to courses and exams
- View and track student exam results

### Student
- Register and log in securely
- Take quizzes and final exams
- View results immediately upon submission
- Attempt multiple quizzes (one final exam attempt allowed per course)

---

## Architecture

The system is built following **Layered Architecture** principles to ensure a clean separation of concerns, high maintainability, and testability.

**Design Patterns Applied:**
- Repository Pattern
- Dependency Injection

---

## Security

| Feature | Details |
|---|---|
| Authentication | JWT Bearer Tokens with **Refresh Token** support |
| Authorization | Role-based access control (Instructor / Student) |
| Token Refresh | Secure refresh token rotation to maintain user sessions |
| Access Control | Exam-level security tied to enrolled roles |
| Data Transmission | Encrypted communication across all endpoints |

> **Refresh Token Flow:** Upon login, the API issues both a short-lived JWT access token and a long-lived refresh token. When the access token expires, clients use the refresh token endpoint to obtain a new access token without requiring re-authentication, ensuring a seamless and secure user experience.

---

## Tech Stack

- **Framework:** ASP.NET Core Web API
- **Language:** C#
- **ORM:** Entity Framework Core
- **Database:** SQL Server
- **Query Language:** LINQ
- **Authentication:** JWT + Refresh Tokens
- **Validation:** FluentValidation

---

## Business Logic Highlights

- Automatic exam generation with difficulty-level balancing
- Role-based resource visibility (instructors see only their own content)
- Exam type enforcement (Quiz vs. Final Exam)
- Student attempt restriction logic (single final exam attempt per course)

---

## Non-Functional Requirements

- Optimized API response times and efficient database queries
- Clean, well-structured, and documented codebase
- Designed for horizontal scalability

---

## Planned Enhancements

- [ ] Timer-based exam sessions
- [ ] Pagination and advanced filtering
- [ ] Analytics dashboard for instructors
- [ ] Email notifications for exam assignments and results

---

## Author

**Ayman Hassan**  
Full Stack Developer passionate about building scalable, secure, and maintainable systems.
