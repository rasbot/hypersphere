<p align="center">
  <img src="https://raw.githubusercontent.com/rasbot/hypersphere/master/Images/cover_image.gif" width="850" height="auto"/>
</p>

Hypersphere is a high immersion VR game for the HTC Vive. The player starts on a platform and can grab and throw the game ball. The game objective for each level is to get the ball from the platform to the target. Once the ball is in free fall and has left the player's grasp, it has to move through a series of game objects like platforms and tractor beams to get to the target. 

## Game Objectives

The conditions to beat each level are:

1. The ball must be thrown from the platform.

2. The ball cannot hit the floor at any time after it has been released.

3. The ball must hit each orb along the path to be valid to hit the target to win.

4. The ball must hit the target.

The player has the ability to walk around the environment and can use a teleport locomotion. The HTC Vive allows for translational and rotational motion, so the controllers and headset can move and rotate in the virtual environment. The starting position platform where the player will begin the game looks like:

<p align="center">
  <img src="https://raw.githubusercontent.com/rasbot/hypersphere/master/Images/ball.gif" width="850" height="auto"/>
</p>

The rotating orbs in the background are the collectibles, and the green platform on the ground is the target. The game has a tutorial level, and 4 levels with different puzzles. If the ball hits the floor at any time it will turn red and the ball will be reset. 

<p align="center">
  <img src="https://raw.githubusercontent.com/rasbot/hypersphere/master/Images/ball_ground.gif" width="500" height="auto"/>
</p>

## Spawnable Game Objects

The player will need to spawn game objects to use to complete the levels. The spawnable objects are:

1. Pulse Bounce (space trampoline)

2. Tractor Beam

3. Plank

4. Funnel

The spawnable objects can be selected on the controller, and will show a small 3D model of the object above the controller. Once spawned, the object can be freely moved anywhere in the level.

<p align="center">
  <img src="https://raw.githubusercontent.com/rasbot/hypersphere/master/Images/spawn.gif" width="700" height="auto"/>
</p>

## Gravity Toggle

To make the game more interesting, a gravity toggle must be used for certain parts of the levels. The button will change the direction of the gravitational force from downward to upward. Toggling it again will reverse it back to downward.

<p align="center">
  <img src="https://raw.githubusercontent.com/rasbot/hypersphere/master/Images/controls.png" width="800" height="auto"/>
</p>

There is also a toggle to reset the scene if the ball happens to get stuck on a surface and doesn't hit the ground (which will auto reset the ball). The button is green when gravity is downward, and red when it is upward.

<p align="center">
  <img src="https://raw.githubusercontent.com/rasbot/hypersphere/master/Images/reverse_g.gif" width="600" height="auto"/>
</p>

## Game Build

To play this game, download the zip file located in the following Google Drive link:

https://drive.google.com/file/d/1Ds9n0ta-kSU8mnUpRB6_04d5MVLOvCvq/view?usp=sharing

This is a Windows build that uses SteamVR, so the player must have Steam and SteamVR installed. This build is specific to the HTC Vive since the controls are mapped to the Vive controllers.

The exe file will launch SteamVR, and the game itself.
