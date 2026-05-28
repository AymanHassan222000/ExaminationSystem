🧠 Online Examination Management System

A scalable web-based Examination and Quiz Management System that enables instructors to create and manage courses, quizzes, and final exams, while allowing students to participate in exams and view their results.

The system is designed with clean architecture principles, focusing on maintainability, scalability, security, and performance.

🚀 Features
👨‍🏫 Instructor

Create, edit, and delete courses

Create quizzes and final exams

Add and manage questions with difficulty levels (Simple, Medium, Hard)

Reuse questions across multiple exams

Manually assign questions to exams

Automatically generate exams with balanced difficulty distribution

Assign students to courses and exams

View students' exam results

👨‍🎓 Student

Register and log in securely

Enroll in assigned courses

Take quizzes and final exams

View exam results upon submission

Attempt multiple quizzes (only one final exam allowed)

🏗️ Architecture

The system follows Onion Architecture principles to ensure:

Separation of concerns

High maintainability

Testability

Scalable structure

Implemented patterns:

Repository Pattern

Dependency Injection

🔐 Security

JWT-based Authentication

Role-Based Authorization (Instructor / Student)

Secure exam access control

Encrypted data transmission

⚙️ Tech Stack

ASP.NET Core Web API

Entity Framework Core

SQL Server

C#

LINQ

JWT Authentication

📊 Business Logic Highlights

Automatic exam generation with difficulty balancing

Role-based access control

Instructor-specific resource visibility

Exam type restriction (Quiz / Final)

Student exam attempt control logic

📈 Non-Functional Requirements

Optimized performance and API response handling

Clean, maintainable, and well-documented codebase

📌 Future Enhancements

Timer-based exams

Pagination & filtering

Analytics dashboard

Email notifications

👨‍💻 Author

Ayman Hassan

Passionate about building scalable and secure backend systems.
