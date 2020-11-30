# Project Documentation

-   Name: Dean Jones
-   Section: 06
-   Assignment: Project 3: HvZ part 1

## Description

This project is a humans vs zombies game using the pursue, separate, evade, obstacle avoidance, wander and edge avoidance algorithms. Zombies pursue the closest humans, who evade all zombies, and when they collide, the human is removed and a zombie is created.

## User Responsibilities

The user can press D to toggle debug mode, which displays debug lines showing the forward, right, and target vectors.
The user can also press Z/X to add and remove zombies, A/S to add and remove humans, and Q/W to add and remove obstacles to affect the simulation partway through.
Cameras can be toggled using 1 for the standard view, 2 for the overhead view, and 3 to enter a "first person mode" which can be cycled through using the left and right arrow keys.

## Above and Beyond

The above and beyond features I have added in this project are the adding and removing of the zombies, humans, and obstacles, as well as the camera options, specifically the "first person" mode which can cycle through each zombie and human to show the simulation from their perspective.

## Known Issues

Under heavy load, sometimes the first person view will allow the zombie or human's head flicker rapidly.
Occasionally, if multiple zombies are spawned in (either by a human dying or pressing z) at the same time, the program may crash.

## Requirements not completed

I completed all project requirements.

## Sources

Zombie Prefab by Pxltiger on the Unity Store<br>
Adventurer Blake Prefab by ManNeko on the Unity Store<br>
Mobile Tree Package by Laxer on the Unity Store<br>
Chainlink Fences by Kobra Game Studios on the Unity Store<br>
Grass Texture from Zee Que on Pinterest<br>

## Notes
For Project 4, I expanded on the work I did in Project 3, noted here: "I used a scenemanager object and script to control instantiation of each human and zombie, and to assign necessary variables to each object as well. Collision detection and handling is also done in this script, using circular collisions, as the humans and zombies can rotate, making AABB collisions impractical. Zombie steering in the calcsteeringforces works by determining the closest human and seeking them. Human steering in the calcsteeringforces works by looping through all zombies, and fleeing each zombie in the range. If no zombies are within 10 units (chosen because a distance under 10 made the humans too easy to catch, and a distance over 10 would cause the humans to cluster in a corner), the human will wander. The wandering algorithm was created inside of the calcsteeringforces for the human by choosing a random point towards the center of the park that the human will walk towards. When the human gets close enough to their desired point, the point will change to a new random point. For both zombies and humans, if none of the other type of object exists, they return to a default movement. For zombies, this is decelerating to a stop, and for humans, this is wandering. The StayInBounds method works by detecting if a vehicle is close to a particular wall, and if so, repelling the vehicle in the direction opposite of that wall, with increasing strength the closer the vehicle gets to the wall. The StayInBounds method also gains increasing strength depending on the magnitude of the calcsteeringforces acceleration to prevent a human from leaving the park if every zombie is chasing it. This will likely be replaced by the weighting implemented in the final version of the Humans vs Zombies project."

In project 4, I first changed the Seek and Flee calls in CalcSteeringForces for both Zombies and Humans to Pursue and Evade, which are methods similar to seek and flee but instead utilize the future position of the game object instead of the current position of the game object. I then drew debug lines seeking the future position instead of the current position for the zombies to represent the new pursue algorithm. I then used small sperical game object prefabs to represent the future position of the humans and zombies which are enabled only during debug mode. After that, I implemented the separation function so that the humans and zombies wouldn't stack on top of each other. I decided that the range for this method would be 8 units, as the force of the separation method on 2 objects that far apart would be minimal anyway. I weighted the flee from each nearby object of similar type by 1 over the distance between the 2 objects. I then implemented the wander method, using the center of the circle as the future position and the radius of the circle as half of velocity. When making the obstacle avoid method, I made the forward range of the obstacle scan equivalent to maxSpeed/mass as that would make their dodging of obstacles similar to their overall agility. When weighting my different behaviors, I doubled the strength of separation and obstacle avoidance as the game objects were regularly overlapping, and halfed the strength of the calcSteeringForces force because the motion of the game object seemed too agile for humans and zombies. When writing the code for adding and removing different objects, I rewrote all the existing Start() code from the scenemanager into individual functions, which could then be called later when triggered by user input. Not only did this allow my Above and Beyond code to work, it also neatened up my code for the sceneManager. For the camera changing code, I used part of the code from Project 1 Random, but for the 3rd camera, the fps camera, I added the ability for it to follow a specific vehicle, and hover slightly ahead of that vehicle's face. I wrote code to allow arrow keys to cycle through the zombies and humans by effectively combining the zombie and human lists by using the human count as a midpoint.
