# Space Game Design Doc

## Overview

![Original concept art for the player and space station ](./concept-art-space-station.png)
![Original concept art for gameplay, showing player, debris, asteroids, aliens, and pirates](./concept-art-gameplay.png)

- Cheesy _Star Fox_-esque character dialog appears:
  - "There's some abandoned treasures floating around out here, but they're drifting away fast!"
  - "There's a reward for you if you can track them down. But be careful; you're not the only one looking!"

- Player is a merchant ship flying around to collect abandoned treasure floating in space
  - Once a treasure is collected, it must be returned to the main space station, where it increments their score
  - When player has collected all treasures, they win, but can continue playing to see how long they last (with leaderboard for longest ones)
  - Also leaderboard for who can collect all treasure the fastest
- Player is equipped with an "impulse beam" to pull objects towards them, or push objects away (like gravity gun in _Half Life_)
  - Can be used to pull in desirable things:
    - Pulling in treasures to collect them
    - Useful "space debris" is floating around, including "power-ups" like engine parts to make ship accelerate faster,
      shield generators, beam amplifiers to extend impulse beam's range, etc.
  - Or push things away
    - Other useless space junk that would damage hull
    - Small asteroids (large ones cannot be moved)
    - Projectiles from enemies
- Enemies/obstacles include:
  - Asteroids (some big and sessile, others small and moving) that damage shields/hull on impact
  - Space pirates with space hooks that can capture treasure, even stealing them from player.
     They will also shoot player with lasers/missiles if player is in view, but their ships are bigger and less maneuverable
  - Space aliens living inside visually distinct asteroids that will attack anything in range
  - Difficulty scales overtime by increasing enemy/obstacle spawn rate

## Mechanics

### Movement

### Health

At game start, debris should be randomly spawned with roughly uniform distribution, centered on space station and player.

Over the course of the game, "spawner" should spawn in an "alternating circle" pattern to keep things well distributed on the map
(spawn in one radian, then the opposite of the map, then rotate N degrees, spawn in that radian, then the opposite side of the map, rotate N degrees, etc.).

Spawn probability should increase with radius from space station, creating a "central safe zone" and suggesting "wild space" beyond.
Probability should also increase over the course of the game (or at least after each treasure collection) to increase difficulty,
up to configurable max probabilities, or max numbers of items.

### Impulse beam

Player is just a merchant ship so they have no guns. They must avoid enemies/obstacles through superior maneuvering,
hiding inside abandoned alien holes where pirates can't fit, luring pirates into alien ambushes, etc.
Player's only real defense is the impulse beam, which can be used to "fire" debris at enemies.

Beam has a range. Left-click-holding will apply a physics force to push, right-click-holding will apply a physics to pull.
The longer button is held, the longer force will be applied.
However, because this is space, the objects will continue moving with that velocity until they hit something else.

In addition to their Rigidbody components, all physics objects will have colliders:

- Normal colliders that cause damage on impact
- "Trigger" colliders that cause the item to be collected, in the case of collectible power-ups

All physics objects will be deleted from the world on impact, with a particle effect, as if the object was destroyed.

Spaceship steers like an actual spaceship, sort of. It can be accelerated in any of the 4 cardinal directions using WASD or arrow keys,
and will move with that velocity until acted on by another force (bumping into something, being shot by pirates or attacked by aliens, steering in another direction).
It will also slow down overtime (which isn't realistic, but whatever, we can say its drag from a molecular gas cloud or something).
In Unity, we'll simply have gravity turned off, and possibly physics materials to remove all friction, so there are no other external forces to worry about.

There could be an "afterburner" effect so player can temporarily boost, maybe only after they've collected a power-up.

### Enemy pathfinding

Two types:

- "Directed": enemies follow a configurable set of waypoints, and just cycle between them. This will give the impression of enemies defending a position.
- "Wandering": these enemies randomly pick a direction and distance, follow that vector, then repeat. This will give the impression of enemies "searching space".

Both of these movement types can use navigation meshes to avoid other objects of the same type, and large immovable objects like large asteroids.

"Engaged" states when enemies are within certain range of key game objects. They will cease their previous behavior and then:

- Near treasure, pirates will move towards the treasure and collect it with their space hooks,
   then start moving away from the player with increased speed and disappear once out of view, as they "get away with their prize".
   If player pursues, these ships will move to the edge of the map and start circling it, shooting at player all the while (only with lasers).
- Near the player, pirates will start shooting volleys of lasers at player.
   They will try to stay moving in a circle of configurable radius around the player:
   close enough to shoot, but far enough to make it difficult for players to retaliate by "pushing" things.
   On a 60-second timer (perhaps indicated by "charging" progress bar to inform players), pirates will also shoot a guided missile that follows player,
   but has limited turn radius and won't avoid obstacles.

At all times, pirates will be "avoiding" large obstacles using the nav mesh logic described above.
They will just smash thru small asteroids and space junk, allowing their shields to take the hit (and hopefully appearing more menacing in the process).

### Spawning

## Leaderboards

We can maintain leaderboards of the following stats, to give players some friendly competition:

- Fastest treasure collection
  - When player collects their final treasure, we save their current [Time.timeSinceLevelLoad](https://docs.unity3d.com/ScriptReference/Time-timeSinceLevelLoad.html) value

## Sound Design

For demo purposes, we only really need one track of 1-3 minute music playing on a loop. Any royalty-free music is fine.
If there's time, we could have the music change and get more intense as more treasure is collected, or as enemies are encountered, etc.

Required sound effects:

- Enemies:
  - Looping sound for pirate engines
  - Laser bolts
  - Looping sound for missile engine
- Player:
  - Looping sound for player engine (only plays while player is accelerating/providing input though)
- Events:
  - Metallic clank when space junk collides with player, pirates, or other junk
  - "Power up" sound when player collects power-ups
  - "Tada" sound when player collects treasures, and again when they're dropped off at the space station
  - "Uh oh" sound when pirates collect treasures
  - Explosion sound when pirates or player are killed
  - "Roar" when aliens pop out of asteroids

## Visual Design

Both players and pirates have separate "shield" and "armor" health meters. Shields are depleted first, but missiles damage player's ship's armor directly, regardless of shields.

Replenishing:

- Shields replenish slowly overtime.
- Armor _could_ replenish after a treasure is brought back to the space station, but that might make things too easy...
- Shield generator power-ups can extend the length of the shield meter
  - Once shields reach some max length, we should no longer spawn shield generator power-ups
- "Armor" power-ups can extend the length of the armor meter
  - Once armor reaches some max length, we should no longer spawn armor power-ups

Additional effects should turn on as ships get damaged:

- At 75%, add "smoke" particle effects
- At 50%, add "spark" particle effects, maybe also change ship sprite to have cracks or appear muted in color
- At 25% add "fire" particle effects, and maybe little "pieces" falling off (without actually changing primary sprite)

Note that objects other than players/pirates/aliens do not have health, they are instantly "destroyed" on contact.
Large asteroids can not be damaged or destroyed (but can emit particles when shot).

When pirates reach 0%, they stop firing, tremble for a second while flashing red, then explode.
I.e., we show an explosion sprite with smoke/fire particles, and spawn some "fragment" sprites that move away from the explosion and slowly fade out.

When player reaches 0%, the game freezes for a second while player flashes red, then player explodes (like pirates) as time moves in slow motion.
Then game over screen is displayed.

### Map

Central Space station. Dense ring of asteroids around edge of map.
We can manually place large asteroids to make the map feel balanced and assist navmesh generation, smaller objects/enemies will all be procedurally placed.
A "high-res pixelated" look would fit the indie sci-fi feel that we're going for, similar to Subset Games products like
[_FTL: Faster Than Light_](https://store.steampowered.com/app/212680/FTL_Faster_Than_Light/) or
[_Into the Breach_](https://store.steampowered.com/app/590380/Into_the_Breach/)_.
For demo purposes, basic greyboxing is fine.

We will need the following sprites/textures:

- Enemies:
  - Pirate ships
  - Laser bolts
  - Missiles (or any additional weapons)
- Player:
  - Spaceship (possibly multiple if we want to allow customization)
- Environment:
  - Space station
  - Tileable "space" background
  - One or two "small" asteroids
  - Tileable asteroid texture for large asteroids
  - 3-5 "space junk" images, e.g. bent pipe, wrench, drill, helmet, solar panel, scaffolding
  - Black holes
- Power ups:
  - Simple greyscale power up "outline" that can easily have color changed
  - Engine boost icon
  - Beam amplifier icon
  - Shield generator icon
  - Reinforced armor icon
- Particles
  - Impulse beam
  - Smoke for pirate ship engines
  - Flames for explosions (when player or pirate ships explode)
  - Sparks from laser bolt impacts
  - "Shield" particles around ships
  - "Tada" particles when player collects a powerup or treasure
- UI
  - Basic button backgrounds
  - Health bar sliders

## UI

### Diegetic UI

This is UI that is "part of the game world":

- Shield and health sliders parented to player
- Shield and health sliders parented to pirates
- "Charging meter" parented to pirates as they prepare to fire missiles
- Arrow pointing back at space station, parented to player

### Non-diegetic UI

This is the typical "overlay" UI that is _not_ part of the game world:

- Number of treasures collected (don't show until first treasure is collected)
- Power-up icons show in top-right as they are collected/won
- Name of game at top of screen?
- Pause menu when player presses escape, with options:
  - Restart
  - Leaderboards (literally just reloads the Unity scene)
  - Settings (placeholder)
  - Exit
- Gameover menu, with options:
  - Background image showing destroyed player spaceship if they lost, or a space station full of treasures if they won
  - Name textbox, for leaderboards, with "Save to leaderboard" button
    - Similar to old arcade machines, nothing stops two different players entering the same name
    - Player is not _required_ to save their info; they can just not press this button
  - Play again (literally just reloads the Unity scene)
  - Leaderboards
  - Exit

## Development Milestones

1. Having player ship move around, with treasures to collect and debris/asteroids to avoid
2. Procedurally spawning stuff overtime
3. Adding pirates for additional challenge
4. Adding aliens for some sick NPC battles
5. Leaderboards (at least single player leaderboard)
