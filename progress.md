# Progress Report - Unity Project

🏗️ **Project Overview**  
**Project Name**: The Line  
**Genre**: Casual  
**Platform**: PC  
**Development Status**: In Progress  
**Last Updated**: 05/01/2025

---

📅 **Milestones**

### Completed Milestones

#### [Line Drawing with Self-Intersection and Scene Transitions]  
**Date**: 23/12/2024  
**Changes/Features Implemented**:  
1. Added functionality to draw a line.  
2. Implemented self-intersection detection for the drawn line.  
3. Ensured the line is only drawn starting from the initial point.
4. Added logic to check if the line connects back to the endpoint. 
5. Add scene switching logic when level is won.
6. Add MainMenu scene.

**Challenges**:  
- Working with coding conventions.  
- First time developing a 2D Unity game.  
- Implementing line drawing functionality.  

**Outcome**: Goal partially achieved (some drawing line problems).

#### [UI Improvements and Countdown Functionality]  
**Date**: 26/12/2024  
**Changes/Features Implemented**:  
1. Improved main menu and level UI design.  
2. Implemented new countdown functionality for gameplay.  

**Challenges**:  
- Finding a coherent UI design.  
- Managing the time cycle effectively.  
- Overcoming time constraints during development. 

**Outcome**: Goal partially achieved (some time problems).

#### [Obstacles, Level Design, and Game Flow Enhancements]  
**Date**: 27/12/2024  
**Changes/Features Implemented**:  
1. Added obstacles to challenge the player.  
2. Improved overall level design for better gameplay.  
3. Implemented win/lose flow mechanics.  
4. Added restart functionality to reset the game after a win/lose condition.

**Challenges**:  
- Coding the obstacle detection system.  
- Ensuring the win/lose flow works seamlessly with the restart logic.

**Outcome**: Goal successfully achieved.

#### [Timer Functionality, Project Organization, and Script Improvements]  
**Date**: 28/12/2024  
**Changes/Features Implemented**:  
1. Added timer icon that subtracts time upon touch.  
2. Improved folder structure for better organization of the Unity project.  
3. Enhanced readability and structure of scripts for easier maintenance.

**Challenges**:  
- Refactoring existing code.  
- Conducting research on Unity project organization and best practices.

**Outcome**: Goal partially achieved (DrawingLine script partially improved).

---

🛠️ **Development Progress**

#### [Feature 1: Drawing Line]  
**Description**: Implementing the ability to draw a line using the mouse.  
**Progress**: 80% complete  
**Issues**:  
1. A new line can be drawn from the starting point without restrictions.  
2. Once the line is released, it is not possible to continue drawing from the endpoint.  
**Solution**:  
1. Accepted the behavior of starting a new line from the beginning.  
2. Decided not to implement the feature for resuming the line from the endpoint after release.

#### [Feature 2: Obstacles]  
**Description**: Implemented obstacles into the game levels.  
**Progress**: 100% complete  
**Issues**:  
1. Perfecting collision detection between the line and obstacles.  
**Solution**:  
1. Worked with Polygon Collider 2D and rewrote scripts to improve accuracy of collision detection.

#### [Feature 3: Time Collectibles]  
**Description**: Time collectibles that allow you to reduce the registered time.  
**Progress**: 100% complete  
**Issues**: /  
**Solution**: / 

#### [Feature 4: Line Markers]  
**Description**: Implementing start and end points for the drawn line.  
**Progress**: 90% complete  
**Issues**:  
1. The line can only be drawn starting from the start point.  
2. Upon reaching the end point, the scene should transition to the next level.  
**Solution**:  
1. Utilized a `LineMarkersManager` script to manage the start and end points.  
2. Combined the `LineMarkersManager` script with a `LoadNextLevel` script to handle scene transitions.

---

📝 **Roadmap**

- **Next Feature to be Added**: Camera that follows the mouse to make levels feel more dynamic.
- **Next Milestone**: Reach 50 levels.
- **Critical Bugs to be Fixed Before Next Release**: Fix timer display in Main Menu and resolve Leaderboard issues.

---

⚙️ **Tools and Resources**  
**Unity Version:** Unity 2022.3.30f1 
**External Libraries/Assets:** TMP Essential Resources 