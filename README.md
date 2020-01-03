# PPSDemo
Example Unity project using the [Processor-Profile System pattern](https://github.com/rellfy/PPS.git)

![Banner](https://cdn.discordapp.com/attachments/607007438180450305/662455141320097802/unknown.png)

## About
**Unity version: 2019.30f3**. Downgrading should not be a problem for recent versions.

This is a simple shooter game. All characters are rolling balls with a gyroscopic turret.
Enemies and health potions spawn randomly within the player's radius.

## PPS Implementation
This project has three* Systems/MonoBehaviours: 

- [World](https://github.com/rellfy/PPSDemo/tree/master/Assets/PPS%20Demo/Systems/World): All physical/visible entities and subsystems are handled by the World system.
- [UI](https://github.com/rellfy/PPSDemo/tree/master/Assets/PPS%20Demo/Systems/UI): System dedicated to firing UI events from World system's events.
- [Audio](https://github.com/rellfy/PPSDemo/tree/master/Assets/PPS%20Demo/Systems/Audio): System dedicated to firing audio clips from World system's events.
- [Network](https://github.com/rellfy/PPSDemo/tree/master/Assets/PPS%20Demo/Systems/Network): This has been left blank as it is not implemeneted, but would be the fourth essential System in case of multiplayer.

\**when counting MonoBehaviours, it is disregarded MonoBehaviours that only act as a delegate for handling Collision events (i.e. [CollisionDelegate.cs](https://github.com/rellfy/PPS/blob/master/Runtime/Utils/CollisionDelegate.cs)), as Unity does not allow for that to be done through the Collider component directly*
