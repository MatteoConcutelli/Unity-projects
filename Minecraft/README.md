# Minecraft 

![alt text][img]

[img]: https://github.com/MatteoConcutelli/Unity_projects/blob/b74ce4275d90bc9e13e265fdf0fc33d66c10c8f9/Minecraft/img.jpg

This is a little copy of Minecraft. It is generated, only, a world 128*128, but these values can be changed in the *World.cs*, this script is for creating the world. 

Blocks can be placed and removed with mouse buttons. You cannot select the blocks to use unless they are changed in the code of the script : 

- https://github.com/MatteoConcutelli/Unity_projects/blob/77062a09912a62ec8b0da3c6e110ef327ad18074/Minecraft/Assets/Scripts/GameManager.cs#L101

In this case the stone block is selected.

All defined block types are in :
- https://github.com/MatteoConcutelli/Unity_projects/blob/77062a09912a62ec8b0da3c6e110ef327ad18074/Minecraft/Assets/Scripts/Blocks.cs#L18-L22
---
### Optimizations

*ChunkManager.cs* and *ChunkCreator.cs* in addition to creating the chuncks, they also take care of not rendering the faces that are not visible, such as those of the blocks under the ground.
