Endless Runner - Focal Length Perspective
This is an Endless Runner game built in Unity using a custom focal length perspective system. All movement, scaling, and collision detection is handled using pure vector math and scalar operations—no Rigidbodies or standard physics components are used.

Game Mechanics
   3-Lane Movement: Switch between 3 lanes using "A" / "D" or the "Left" / "Right" arrows.
   Jumping: Dodge obstacles by jumping with "W", "Space", or the "Up" arrow.
   Health System:
       Base HP: 100
       Damage: -5 HP per hit.
       Regeneration: +1 HP per second.
   UI: Real-time HP display on the screen.

Mathematical Implementation
The game uses a virtual Z-depth to calculate perspective. The transformation follows the standard focal length formula:
   Scale: "perspective = focalLength / (focalLength + virtualZ)"
   Position: "screenPos = virtualPos * perspective"
Collisions are calculated by comparing the player's virtual Z-value and lane proximity against oncoming obstacles within a specific range.

Setup & Demo
1.  Open the project in Unity.
2.  Open the main scene.
3.  Ensure the "ObstacleManager" and "FloorManager" are configured in the inspector.
4.  Press Play to start.

Video Demonstration
(https://drive.google.com/file/d/1eToEPcTVUMYz9Ueogda0bqmh_20OPnqN/view?usp=sharing)

Project Scripts
   "CameraComponent.cs": Perspective constants.
   "Item.cs": Handles individual object perspective.
   "PlayerController.cs": Input, movement, and health.
   "ObstacleManager.cs": Spawning and collision math.
   "FloorManager.cs": Endless scrolling floor logic.
   "HPUI.cs": UI health bar management.