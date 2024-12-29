# Research Notes

This document contains research notes, sources, and findings related to the project.

## Table of Contents
- [Research Notes](#research-notes)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Sources](#sources)
  - [Key Concepts](#key-concepts)
  - [Findings](#findings)
  - [Further Research](#further-research)

---

## Introduction

The purpose of this research is to explore effective strategies for developing 2D and 3D games in Unity. This research is focused on various key elements such as sprite management, prefab usage, database integration, and optimization techniques. By consolidating industry best practices and Unity's built-in tools, the research aims to provide a comprehensive guide for developing scalable, efficient, and maintainable games in Unity. The findings will inform game development workflows, ensuring best practices are followed from coding and asset management to implementing online features like leaderboards.

---

## Sources

- **[Source 1: Naming and code style tips for C# scripting in Unity](https://unity.com/how-to/naming-and-code-style-tips-c-scripting-unity)**
- **[Source 2: Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)**
- **[Source 3: Unity documentation prefabs](https://docs.unity3d.com/Manual/Prefabs.html)**
- **[Source 4: How to make a 2D Game in Unity](https://www.youtube.com/watch?v=on9nwbZngyw)**
- **[Source 5: How To Draw a Line in Unity | Line Renderer Tutorial 1](https://www.youtube.com/watch?v=5ZBynjAsfwI)**
- **[Source 6: How to Draw in Unity using Line Renderer | Unity Tutorial](https://www.youtube.com/watch?v=M4247oZ8sEI)**
- **[Source 7: Unity Make Inspector Look Clean](https://stackoverflow.com/questions/76920864/unity-make-inspector-look-clean)**
- **[Source 8: API Call in Unity](https://medium.com/@hardikparmarexpert/api-call-in-unity-021c3df18429#:~:text=To%20make%20API%20calls%20in,use%20it%20in%20their%20projects.)**
- **[Source 9: ChatGPT best database for Unity](https://chatgpt.com/c/6771d532-fa24-8003-a746-e4c5ca88d5df)**
- **[Source 10: Introduction to Connecting Unity to MySQL Database](https://www.youtube.com/watch?v=khmqwgQVv2A)**
- **[Source 11: Realtime Database in UNITY - FIREBASE TUTORIAL](https://www.youtube.com/watch?v=hAa5exkTsKI)**
- **[Source 12: Best database for a Unity3D PC game](https://discussions.unity.com/t/best-database-for-a-unity3d-pc-game/918627)**
- **[Source 13: General question about databases in Unity](https://www.reddit.com/r/Unity3D/comments/scj9u5/general_question_about_databases_in_unity/?rdt=60768)**
- **[Source 14: Unity Sample Project](https://github.com/heroiclabs/unity-sampleproject/blob/master/README.md)**
- **[Source 15: Sending and Requesting Data from MongoDB in a Unity Game](https://www.mongodb.com/developer/languages/csharp/sending-requesting-data-mongodb-unity-game/)**
- **[Source 16: Naming Conventions for Prefabs, Materials, Shaders, etc.](https://discussions.unity.com/t/naming-conventions-for-prefabs-materials-shaders-etc/476894)**
- **[Source 17: 30 UNITY Tips for 2D Games!](https://www.youtube.com/watch?v=gg07tzbj2pU)**
- **[Source 18: Really confused about prefabs](https://www.reddit.com/r/Unity3D/comments/lg1jon/really_confused_about_prefabs/)**
- **[Source 19: 2D game creation workflow](https://docs.unity3d.com/6000.0/Documentation/Manual/2d-game-creation-wokflow.html)**
- **[Source 20: HOW TO SETUP BUILD SETTINGS | UNITY3D TUTORIAL](https://www.youtube.com/watch?v=KcCOILyuAlk)**
- **[Source 21: Tips for Good Unity Programming?](https://www.reddit.com/r/Unity3D/comments/122t8ju/tips_for_good_unity_programming/)**
- **[Source 22: Scene Manager in Unity (Loading Unity Tutorial)](https://www.youtube.com/watch?v=3I5d2rUJ0pE)**
- **[Source 23: Unity Firebase Database Integration - Easy Tutorial](https://www.youtube.com/watch?v=59RBOBbeJaA)**
- **[Source 24: How to make an Online Leaderboard in Unity for Free!](https://www.youtube.com/watch?v=-O7zeq7xMLw)**
- **[Source 25: Unity documentation Leaderboards](https://docs.unity.com/ugs/manual/leaderboards/manual/leaderboards)**
- **[Source 26: Use Leaderboards to amp up competition](https://unity.com/products/leaderboards)**
- **[Source 27: Using Git source control in VS Code](https://code.visualstudio.com/docs/sourcecontrol/overview)**

---

## Key Concepts

In this section, I will outline the key concepts I discovered during my research. These include definitions, methodologies, and ideas central to the research.

- **Sprite Management**: Handling 2D sprites in Unity, including animation, sprite sheets, and sprite rendering. Ensuring efficient use of resources and managing sprite atlases for optimization.
- **Prefabs**: Prefabs are reusable game objects or components that can be instantiated multiple times within the Unity editor, making them a crucial part of scalable game design.
- **Database Integration**: Understanding how to link Unity with different databases, including MySQL, MongoDB, and Firebase, for storing game data such as player progress, leaderboards, and online features.
- **Leaderboard Setup**: The process of setting up online leaderboards within Unity games, including API calls, Firebase setup, and displaying real-time player data.
- **Clean Code Writing**: Best practices for writing readable, maintainable, and efficient code in Unity, focusing on consistency, structure, and scalability.
- **Clean Folder Setup**: Organizing project folders effectively within Unity, making it easier to maintain assets, scripts, and scenes, thereby improving collaboration and efficiency.
- **General Guidelines for Coding a Game**: A set of industry standards and best practices for coding in Unity, such as using object-oriented principles, optimizing performance, and following Unity’s naming conventions.

---

## Findings

Here are some key findings from the research:

- **Finding 1**: Unity's built-in 2D tools, such as the Sprite Renderer, will significantly reduce development time for 2D games by handling rendering and animation.
- **Finding 2**: The use of game design patterns like the Component-Entity pattern and Prefabs helps create scalable and maintainable game systems in Unity.
- **Finding 3**: Understanding time management in game development is crucial, especially when handling frame rates and managing real-time events in Unity.
- **Finding 4**: The general setup of a Unity 2D game involves setting up sprites, animations, prefabs, and game logic in a coherent workflow.
- **Finding 5**: Learned how to link a Unity game with an external database using Firebase or MySQL for storing and retrieving player data.
- **Finding 6**: To control a database in Unity, it is essential to set up API calls to manage data requests efficiently, reducing the load on the game’s core system.

---

## Further Research

Here are some open questions for further research:

- **Question 1**: How can we optimize Unity game development in both 2D and 3D to improve performance, especially on lower-end devices?
- **Question 2**: What are the best practices for managing large sprite sheets in Unity to improve rendering performance and memory usage?
- **Question 3**: If you have many objects with names like `name_000`, is it possible to automate the naming process using scripts to reduce manual effort, or is it better to do it manually?