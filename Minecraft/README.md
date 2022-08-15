# Minecraft 

This is a little copy of Minecraft. It is generated, only, a world 128*128, but these values can be changed in the *World.cs*, this script is for creating the world. 

Blocks can be placed and removed with mouse buttons. You cannot select the blocks to use unless they are changed in the code of the script : 

- https://github.com/MatteoConcutelli/Unity_projects/blob/4320a829a928f20ef42cd44d8327784c8b3f1fca/Minecraft/Assets/Scripts/GameManager.cs#L101

In this case the stone block is selected.

All defined block types are in :
- https://github.com/MatteoConcutelli/Unity_projects/blob/a1be1ed673b4aff5db9813c8910990a23272908c/Minecraft/Assets/Scripts/Blocks.cs#L16
---
### Optimizations

*ChunkManager.cs* and *ChunkCreator.cs* in addition to creating the chuncks, they also take care of not rendering the faces that are not visible, such as those of the blocks under the ground.
