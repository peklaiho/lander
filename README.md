# Lander

![Lander screenshot](/img/lander-screenshot.png)

My take on the classical [Lunar Lander](https://en.wikipedia.org/wiki/Lunar_Lander_%28video_game_genre%29) game. The objective of the game is to find a flat spot and land on the surface of Europa (moon of Jupiter).

The landing is successful if at least half of the lander is located on flat surface. Also horizontal and vertical velocity needs to be within allowed limits (depends on difficulty). The lander is affected by gravity and wind which are slightly different for each game. The lander is controlled by firing the main thruster to increase vertical velocity and side thrusters to increase horizontal velocity.

The game has three difficulty levels and also a mode where the computer plays the game.

## Running

Install .NET SDK and SDL 2 libraries. For example on Arch Linux:

```
$ pacman -S dotnet-sdk sdl2 sdl2_ttf
```

Then run the game:

```
$ dotnet run
```

## Keys

- F1: New game (easy)
- F2: New game (normal)
- F3: New game (hard)
- F4: New game (cpu)
- s or ↓: Fire main thruster (move up)
- a or ←: Fire left thruster (move right)
- d or →: Fire right thruster (move left)
