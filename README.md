# Project Documentation

-   Name: Dean Jones
-   Section: 06
-   Assignment: Project 3: HvZ part 1

## Description

This project is an early version of a humans vs zombies game using the seek and flee algorithms. Zombies seek the closest humans, who flee all zombies, and when they collide, the human is removed and a zombie is created.

## User Responsibilities

The only user interaction is the ability for the user to press D to toggle debug mode, which displays debug lines showing the forward, right, and target vectors.

## Above and Beyond <kbd>OPTIONAL</kbd>

Not required for this project.

## Known Issues

There are no known issues in the program.

## Requirements not completed

I completed all project requirements.

## Sources

All sprites and scripts were created by me. There are no unoriginal components in the project.

## Notes
I used a scenemanager object and script to control instantiation of each human and zombie, and to assign necessary variables to each object as well. Collision detection and handling is also done in this script, using circular collisions, as the humans and zombies can rotate, making AABB collisions impractical. Zombie steering in the calcsteeringforces works by determining the closest human and seeking them. Human steering in the calcsteeringforces works by looping through all zombies, and fleeing each zombie in the range. If no zombies are within 10 units (chosen because a distance under 10 made the humans too easy to catch, and a distance over 10 would cause the humans to cluster in a corner), the human will wander. The wandering algorithm was created inside of the calcsteeringforces for the human by choosing a random point towards the center of the park that the human will walk towards. When the human gets close enough to their desired point, the point will change to a new random point. For both zombies and humans, if none of the other type of object exists, they return to a default movement. For zombies, this is decelerating to a stop, and for humans, this is wandering. The StayInBounds method works by detecting if a vehicle is close to a particular wall, and if so, repelling the vehicle in the direction opposite of that wall, with increasing strength the closer the vehicle gets to the wall. The StayInBounds method also gains increasing strength depending on the magnitude of the calcsteeringforces acceleration to prevent a human from leaving the park if every zombie is chasing it. This will likely be replaced by the weighting implemented in the final version of the Humans vs Zombies project.
