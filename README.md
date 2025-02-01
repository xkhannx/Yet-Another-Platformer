# Yet Another Platformer (2022)
 
Yet another unfinished project, which was abandoned due to, you know, life. Done in cooperation with talented [Abylay Mukhit](https://abylaymukhit.itch.io/), who was the artist and level designer (we didn't get around to the level design part).
To try this project out, go to GameDesignerFiles/MainMenu.unity and hit Play. This will take you to the level creator, which you can play at any moment.
In-editor controls are in the Legend. In-game controls: WASD to move, Space to jump, X to enter doors.

Feel free to use the code, but don't use the art assets without permission.

Points of interest:
- Level editor
For STYX I've implemented a level editor by customizing the Unity Tilemap tool for the level designer. That tool was capable of building the level geometry, as well as placing level mechanics, such as traps, springs, moving platforms, and teleports. For this project I've decided to create the level editor from scratch, to have more control over it. For ease of use, I've implemented it in Play mode, so that potentially it would be available for player-created levels. All data is stored in Scriptable Objects.
This level editor allows to build/paint level geometry and special blocks on 5 different layers (player sprite is on level 4).

- Shadow shader
For this 2D game I've implemented a shadow shader using HLSL and stencil buffer. Since there are several layers, the shadow needs to cascade down to the layers behind, with appropriate offset and blending with the rest of the shadows.

The projects that we did get around to publish:
- [Hyper Pixel Beast](https://apps.apple.com/us/app/hyper-pixel-beast/id1503065407)
- [STYX: Vertical platformer](https://apps.apple.com/us/app/styx-vertical-platformer/id1587424131)
