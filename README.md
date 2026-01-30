🟢 Habit Tracker System

A full-stack web application that helps users build and maintain daily habits through tracking, reminders, streaks, and analytics.


📌 Project Overview

The Habit Tracker System is designed to help users develop positive daily routines in a structured way. Users can create habits, manage categories, schedule days, set reminders, track daily completion, and monitor streaks. The system also provides an Admin Panel for user monitoring.


🚀 Features

👤 User Side
User Registration & Login (JWT Authentication)

Create / Update / Delete Habits

Create and Manage Habit Categories

Schedule habits for specific days of the week

Set daily reminders

Mark habit status (Done / Skipped / Not Done)

View current and longest streak

Habit analytics dashboard


🛠 Admin Side
Manage users

View active and inactive users


🏗 Tech Stack
Layer	Technology
Frontend	: React JS, HTML, CSS, Bootstrap

Backend	: ASP.NET Core Web API

Database	: MySQL

Authentication :	JWT

ORM	: Entity Framework Core

Version Control	: Git & GitHub


🗂 Project Structure
HabitTracker/
│
├── Backend (ASP.NET Core API)
│   ├── Controllers
│   ├── Models
│   ├── Services
│   ├── DTOs
│   └── Data (DbContext)
│
└── Frontend (React)
    ├── components
    ├── pages
    ├── services
    └── App.js



🗄 Database
MySQL Database

Update connection string in:
appsettings.json


🔐 Authentication
The system uses JWT Token Authentication for secure API access.


📊 Future Enhancements
Mobile Application
AI-based habit suggestions
Push notifications
Gamification (badges, rewards)
Wearable device integration
