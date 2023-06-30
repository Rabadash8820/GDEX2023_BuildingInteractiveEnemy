# Live Demo: Building an Interactive Enemy

## Overview

This repo contains all source code and assets from my GDEX 2023 presentation _Live Demo: Building an Interactive Enemy_.
View a recording of the talk and/or the accompanying slides on [One Drive](https://tinyurl.com/enemySlidesGdex23).

The Unity project is contained in the `SpaceGame/` folder. Its `Assets/` folder contains subfolders for several third-party assets.
All first-party assets are stored in the `SpaceGame/` subfolder, grouped by "feature" (`player`, `monster`, `world`, etc.).
Open the single scene file at `scenes/main.unity` to see how it's all put together!

The game itself, _"Space Game"_, is basically a treasure hunt through space,
where the player must dodge asteroids and monsters to collect all "treasures" and bring them back to the space station.
Players can also use a simple "gravity gun" to push and pull asteroids out of the way.
Check out the origial [design doc](./original-design-doc.md).

## Gameplay

<img src="./gameplay-moving-impulse-treasure.gif" width="600" height="300"
    alt="Gameplay GIF showing player ship flying through space, dodging asteroids, and collecting treasure"/>
<img src="./gameplay-monster-lasers.gif" width="600" height="300"
    alt="Gameplay GIF showing player ship dodging laser bolts from the eyes of a floating Jeff Bezos head"/>

## Play it

_Space Game_ was meant to be played with a mouse/keyboard on desktop or the web.
Feel free to demo the [zipped Windows x64 build](./builds/win64.zip) from this repo.
Just download, unzip, and run the executable!
