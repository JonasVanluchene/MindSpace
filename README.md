# 🧠 Assignment: MindSpace – A Personal Journal & Mood Tracker
## 📌 Scenario
Build a private web app for users to track their daily moods, write journal entries, and reflect on patterns over time. This is a calm, introspective tool designed to help users understand their mental well-being.

## 🎯 Learning Goals
This project helps you practice:

- Entity Relationships (1-to-many, many-to-many)

- Authentication and Authorization (only view your own data)

- File upload (optional: upload an image per entry)

- CRUD operations with validation

- Using services, DTOs, and ViewModels

- Date filtering and searching

- Optional charting (mood over time)

## 🏗️ Functional Requirements
- Users
  - Register/login/logout

  - Update profile and avatar

- Journal Entries
  - Create/Edit/Delete entries

  - Each entry has:

    - Date

    - Title

    - Content

- Mood (dropdown: Happy, Sad, Angry, Calm, Anxious, etc.)

- Tags (like “Work”, “Family”, “Health” — optional many-to-many)

- Filter by date, mood, or tag

- View mood trends (optional chart)

- Reflection Questions (optional feature)
    - Each day presents a rotating reflection question (e.g., “What made you smile today?”)

- Users can optionally answer it with their entry

## 🗃️ Suggested Data Model
### User (Identity)
- Name

- Email

- AvatarUrl

### JournalEntry
- Id

- Title

- Content

- Date

- Mood (enum or string)

- UserId (FK)

- Tags (many-to-many)

### Tag
- Id

- Name

### JournalTag
- JournalEntryId

- TagId

### ReflectionQuestion
- Id

- QuestionText

- (Optional: DisplayFrequency or DateAdded)

## 🛠️ Technical Requirements
ASP.NET Core MVC

EF Core + Migrations

Services + DTOs + ViewModels

User-specific data: only show entries belonging to the logged-in user

Form validation with annotations or FluentValidation

Partial Views for Journal List, Mood Selector

Optional: chart (e.g. mood over last 30 days using Chart.js or Razor components)

## ✨ Bonus Features
Export journal entries as PDF or text

Mood chart with filter (last week/month/year)

“Daily Reminder” email (requires background tasks/email integration)

Dark mode toggle
