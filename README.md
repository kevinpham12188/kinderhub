# KinderHub

A daycare management platform built as a full microservices study project to demonstrate real-world backend engineering.

## What it does

KinderHub allows daycare administrators to manage classroom enrollment, teacher schedules, and supply tracking. Teachers can view their daily schedule and log supply usage per child. Parents receive automated notifications when their child is checked in, checked out, or running low on supplies. Administrators get a real-time dashboard summarizing attendance, tuition, and salary data.

## Architecture

Seven independent microservices communicating via HTTP and async messaging:

| Service      | Responsibility                                          |
|--------------|---------------------------------------------------------|
| Identity     | Authentication, JWT, user profiles, role management     |
| Enrollment   | Children records, classroom assignments, capacity       |
| Attendance   | Check-in / check-out, daily logs, duration tracking     |
| Supply       | Diapers, wipes, usage logging, low-supply alerts        |
| Schedule     | Teacher shifts, room timetables, coverage logic         |
| Notification | Email, SMS, and push alerts                             |
| Reporting    | Read model, dashboard summaries, classroom roster       |

## Tech Stack

| Layer          | Technology                              |
|----------------|-----------------------------------------|
| Language       | C# / .NET 10                            |
| Databases      | PostgreSQL 16 (one per service)         |
| Caching        | Redis                                   |
| Messaging      | RabbitMQ 3                              |
| API Gateway    | YARP                                    |
| ORM            | Entity Framework Core                   |
| Auth           | JWT + BCrypt                            |
| Testing        | xUnit + Moq                             |
| DevOps         | Docker · Docker Compose · GitHub Actions|
| DB GUI         | DBeaver (Community Edition)             |
| API Testing    | Thunder Client                          |

## Running locally

```bash
docker-compose up
```

All seven services, databases, Redis, and RabbitMQ start with one command.

## Project status

![CI](https://github.com/kevinpham12188/kinderhub/actions/workflows/ci.yml/badge.svg)

---

### Phase 1 — Solution Structure
- [x] Solution scaffold — all 7 services + gateway
- [x] docker-compose — PostgreSQL + RabbitMQ
- [x] GitHub Actions CI pipeline

---

### Phase 2 — Identity Service
- [x] Schema design
- [ ] Register + login endpoints
- [ ] JWT token generation + validation
- [ ] BCrypt password hashing
- [ ] Role-based access — Admin, Teacher, Parent
- [ ] Unit tests — xUnit + Moq

---

### Phase 3 — Enrollment Service
- [ ] Schema design
- [ ] Manage classrooms (create, list, capacity)
- [ ] Enroll a child
- [ ] Assign teacher to classroom
- [ ] Publish `ChildEnrolled` event
- [ ] Unit tests

---

### Phase 4 — Attendance Service
- [ ] Schema design
- [ ] Check-in a child
- [ ] Check-out a child
- [ ] Daily attendance report
- [ ] Publish `ChildCheckedIn` / `ChildCheckedOut` events
- [ ] Unit tests

---

### Phase 5 — Supply Service
- [ ] Schema design
- [ ] Log supply usage per child
- [ ] Configurable low-supply threshold
- [ ] Publish `SupplyLow` event via RabbitMQ
- [ ] Unit tests

---

### Phase 6 — Notification Service
- [ ] Subscribe to `SupplyLow` → alert parent
- [ ] Subscribe to `ChildCheckedIn` / `ChildCheckedOut` → alert parent
- [ ] Email delivery (MailKit)
- [ ] Unit tests

---

### Phase 7 — Schedule Service
- [ ] Schema design
- [ ] Teacher shift management
- [ ] Room timetable
- [ ] Publish `TeacherAssigned` / `ShiftChanged` events
- [ ] Unit tests

---

### Phase 8 — Reporting Service
- [ ] ClassroomRoster read model
- [ ] Subscribe to events from Enrollment, Attendance, Supply, Schedule
- [ ] Dashboard — tuition summary, salary totals, attendance rates
- [ ] Classroom tab — children per class with status + supply level
- [ ] Unit tests

---

### Phase 9 — API Gateway
- [ ] YARP routing across all services
- [ ] JWT forwarding
- [ ] Redis session cache
- [ ] Rate limiting

---

### Phase 10 — Hardening
- [ ] Outbox pattern — reliable event publishing
- [ ] Global exception handling
- [ ] Structured logging — Serilog
- [ ] Distributed tracing — OpenTelemetry

---

### Phase 11 — Testing
- [ ] Integration tests — Testcontainers
- [ ] Contract tests between services

---

### Phase 12 — CI/CD
- [ ] GitHub Actions — build + test on every PR
- [ ] Docker build per service
- [ ] Automated migration on deploy