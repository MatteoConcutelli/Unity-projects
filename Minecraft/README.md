# Minecraft Clone 🧱
<img src="https://github.com/MatteoConcutelli/Unity_projects/blob/b74ce4275d90bc9e13e265fdf0fc33d66c10c8f9/Minecraft/img.jpg" width="500" alt="Minecraft Clone" />

This is a simplified Minecraft clone made with Unity. It generates a 128x128 block world, but you can modify this value in the *World.cs* script. 

🎮 **Gameplay Features:**
- Place and remove blocks using mouse buttons.
- Currently, block selection is hardcoded in:  
  [GameManager.cs](https://github.com/MatteoConcutelli/Unity_projects/blob/77062a09912a62ec8b0da3c6e110ef327ad18074/Minecraft/Assets/Scripts/GameManager.cs#L101)  
- All block types are defined in:  
  [Blocks.cs](https://github.com/MatteoConcutelli/Unity_projects/blob/d5294ff662bfbad3a56543369769c13e95c9f670/Minecraft/Assets/Scripts/Blocks.cs#L18-L22)

---

### 🚀 Optimizations

*ChunkManager.cs* and *ChunkCreator.cs* manage chunk creation and optimize rendering by hiding non-visible block faces, enhancing performance.

---

Feel free to customize or expand it further! 🎨
