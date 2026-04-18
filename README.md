# Hangman

This application is an implementation of the classic **Hangman** game, developed in **C#** using the **WPF .NET Core** framework. The project strictly adheres to the **MVVM (Model-View-ViewModel)** architectural pattern and utilizes **Data Binding** to synchronize the user interface with the underlying business logic.

## 🚀 Key Features

### 1. User Management System (Sign In)
**Account Creation/Deletion**: Users can create new accounts and associate them with a profile image using relative paths.
**User Selection**: Allows selecting existing users from a list or navigating through them using interface buttons (`<<`, `>>`).
**Data Persistence**: User data, image associations, and statistics are stored in external files.
**Dynamic UI**: Buttons like "Play" and "Delete User" are initially disabled and only become active once a user is selected.

### 2. Gameplay Mechanics
**Progression System**: A game is considered won after successfully guessing **3 consecutive words**.
**Word Categories**: Players can choose from various categories or select "All categories".
**Hangman Stages**: Visual progression of the hangman character with each mistake, accompanied by "X" markers for missed lives.
**Timer**: Players must guess the word within a **30-second** time limit.
**Input**: Supports both on-screen button clicks and keyboard shortcuts for letters and menu actions.

### 3. Save System & Statistics
**Save/Open Game**: Allows players to save their current progress (level, remaining time, mistakes, current word state) and resume later.
**Global Statistics**: Displays the number of games played and won for every registered user.
**Full Cleanup**: Deleting a user removes all associated data, including saved games and statistics.

## 🛠️ Technical Specifications
**Architecture**: MVVM (Model-View-ViewModel).
**Commands**: Implementation based on `ICommand`.
**File Paths**: Exclusive use of **relative paths** for images and save files to ensure portability.
**UI/UX**: Designed in XAML with a focus on scannability and professional layout.

## 📂 Project Structure
* `Models/` – Data classes (User, GameSession, Word).
* `ViewModels/` – Business logic and state management (LoginViewModel, GameViewModel, StatsViewModel).
* `Views/` – XAML Windows and UI definitions (LoginWindow, GameWindow, StatsWindow).
* `Services/` – Logic for file handling, word randomization, and statistics tracking.
* `Helpers/` – Utility classes (RelayCommand, BaseViewModel).
* `Resources/` – Static assets (Hangman stage images and user profile icons).

## 🎓 Author
**Name**: Pelin Cosmin
