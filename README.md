# WordQuest
Welcome to the Word Game! This repository contains the source code for a fun and challenging word game built with Unity. 
Objective is to score a target before you run out of moves.
The game features multiple levels, special powers, and various difficulty settings, providing an engaging experience for players of all skill levels.

- To play the game hop over to : [Google play store link](https://play.google.com/store/apps/details?id=com.DefaultCompany.WordQuest)
- To see the code, switch to branch : `releases/android/2.0.0`


## Features

- **100 Levels**: Progress through 100 unique levels, each increasing in complexity and challenge.
- **Powerups**: Utilize special powers to gain advantages and overcome difficult levels.
- **Design Patterns**: The game is designed with clean architecture principles, utilizing the Dependency Inversion and Observer patterns to ensure maintainability and scalability.
- **Firebase Integration**: Integrated with Firebase for tracking exceptions and logging events for analytics.

## Installation

To get started with the Word Game, follow these steps:

1. **Clone the repository**:
    ```bash
    git clone git@github.com:Beelzebubxcrows/WordQuest.git
    cd WordQuest
    ```

2. **Open in Unity**:
    - Open Unity Hub.
    - Click on `Add`.
    - Select the cloned repository folder.
    - Open the project in Unity.
  

3. **Build and Run**:
    - Select your target platform as Android.
    - Configure your build settings.
    - Build and run the game on your selected platform.

## Usage

Once the game is running, you can start playing directly on your device or in the Unity Editor. 


## Design Patterns

### Dependency Inversion Principle

The game architecture follows the Dependency Inversion Principle, ensuring that high-level modules do not depend on low-level modules. This is achieved by:

- **Abstracting Dependencies**: High-level components interact with abstractions (interfaces) rather than concrete implementations.
- **Inversion of Control**: Using dependency injection to provide necessary dependencies to components.

### Observer Pattern

The Observer pattern is implemented to handle events and notifications within the game. Key areas of implementation include:

- **State Changes**: Observers are notified of changes in game state, such as level completion, power usage, and score updates.
- **Real-Time Updates**: Firebase integration leverages the observer pattern to update the game state in real-time, ensuring a seamless experience for players.

## Contributing

Contributions are welcome! If you have suggestions for new features, improvements, or bug fixes, please open an issue or submit a pull request. Follow these steps for contributing:

1. **Fork the repository**.
2. **Create a new branch**:
    ```bash
    git checkout -b feature-name
    ```
3. **Make your changes**.
4. **Commit your changes**:
    ```bash
    git commit -m "Description of changes"
    ```
5. **Push to the branch**:
    ```bash
    git push origin feature-name
    ```
6. **Open a pull request**.

