CJMY Graffiti README


Develoeprs: Cedric Williams, Jonathan Gary Doerr, Martin L, and Yoandry Antonio Sosa


CJMY Graffit Game Purpose
As a graffiti artist, your goal is to tag the city and not get detected. 


Player Control
To move the character, use the arrow keys or W,A,S,Z and hold Shift to run with forward movement.
To spray paint or tag a wall, push Q.
To jump, press Spacebar (not implemented right now)


What to expect while playing
After clicking “Start” from the main menu, you will control a character in third person view.  Goal is to find a spray can and tag designated areas without getting caught by the Cops (Raptors for now).  There’s a 10 minute timer to tag as many walls as possible without getting caught.


Issues
* Right now, we’re missing the final animated NPC Cop and have a substitute as a Raptor.  Also, the state change for the NPC Cop (Raptor) is not functioning correctly.  The raptors are only suppose to attack the Player when the player is spraying.  While walking, the player should be unscathed.
* We need to expand beyond our sample scenario and add more spray cans around the level and NPC enemies on patrol.
* We need to add more sprayable areas in the level.
* We need to add instructions to playing the game in the menu
* The Player animation while turning is wonky and we need to smooth out the transitions.
* Add sound effects while acquiring a spray can
* Add sound effects for when NPC capture you
* Create an animation for when we are caught
* Add Jump functionality for the Player




Contributions from Team Members




Cedric
Added two raycasting rays to represent eyes that look 50 meters straight.  The code is present in the EnemyVision.cs and PlayerState.cs.  The script performs by casting two rays while the cop is patrolling the area.  Once the cop notices Player1 spray painting, he will change state into “chase” mode until he captures Player1.
- Imported and modified  level to the game
- Created Main menu template and integrated main menu into game play. 
- Stitched all components together for a total game experience
- Adjusted UI to fit the screen
- Adjusted the previous script written for coins for Spray cans
- Optimized NPC enemy to work with EnemyVision script
- Project Management
- Added spray paint collectables for collecting in-game
- Created the code to utilize the states for both the enemy and player and trigger the chase interactions
- Architected the game and led the meetings
Files:
* UIManager.cs
* PlayerState.cs
* EnemyVision.cs
* EventManagerScript.cs
* MainMenu Scene


Martin
Added Raptor chasing player, added mesh collider to relevant assets, removed rigidbody from main character due to rigidbody controlled by script. Added base collector(coin collector script) for coin collector mechanics. Added 5 way points to raptor AI (around the pool). Made Raycasting for raptors to follow player when player gets close. Added temporary raptor model and worked on making model animating correct way. Did a lot of project merging across various branches.
- Refactored Raycast for NPC and integrated into main AI code, instead of a separate file originally by Cedric.
- Senior Developer of the group and aided in debugging and refactoring code
Files:
* SprayCan Prefab as well
* NPC AI States
* NEWSPRAYCAN.prefab
* RaptorAI2.cs
* WaypointChase.cs
* itemCollector.cs
* itemPickup.cs




Jonathan
Added the in-game UI with an objective field (to be updated in the future) as well as a score and a timer for counting down. Timer.cs contains the logic for the timer component. Added sound effects for walking.  
- Added all audio.
- Debugged Alpha version after GitHub merging
- Fixed spray can collection
- Fixed NPC animation
Files:  
* Canvas for in-game GUI
* Debugged NavMesh and Waypoints
* WaypointChase.cs
* 



Yoandry Sosa
Added main character model, created animation controller and parameters used in the controller, also created forward blend tree with motion animations for the character including (idle, walk, run, turn left, turn right, walking right and left turns, running right and left turns, and jump which is not yet an action that the character can do) and backward blend tree (with backward walk and run). Created the spray can, the particle spray for the can, and the materials used for both in unity. Implemented the first graffiti template for use, adding the light to mark the spot of animation which will not be completed until later stages in the game, but demonstrates the logic and purpose of the game. Created the Spray animation in the animator by blending animations enabling and disabling features in the game to hide and show objects during the animation process. Wrote the character control script with all main character and object animations, this includes: the code for the main character blend tree, the code for enabling and disabling game objects, and character position controls (spray can, green spray, graffiti, etc.), and the code to design hotkeys to animate the character or switch between walk and run.
- Helped debug GitHub issues from merging
Worked on the AJ Character Animations in the AJ Character folder
Files:
* AJ_controller_Script.cs
* AutonomousMovement.cs
* Created Spray Can using primitives
* SprayCan animation and prefab




Joseph (left group)
Joseph was suppose to do the animations and characters but was no responsive and eventually left the group.  We haven’t heard from Joseph since our last meeting Wednesday, March 6th.  Due to his error, we had to make some changes and use stock characters until we decide how to progress forward.