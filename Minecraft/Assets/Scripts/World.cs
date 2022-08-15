using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    Blocks block = new Blocks();
    Vector3 currentRenderPos;
    bool updateWorld = true;

    public World (Vector3 PlayerPos) 
    {
        currentRenderPos = PlayerPos;
    }

    public void update (ChunkManager sChunk, Vector3 playerPos) 
    {

        if (updateWorld) {

            float xoff = 0;
            for (int z = (int)playerPos.z - 64; z < (int)playerPos.z + 64; z++) {
                float yoff = 0;
                for (int x = (int)playerPos.x - 64; x < (int)playerPos.x + 64; x++) {
                    for (int y = 0; y < 128; y++) {

                        int noise = (int)(112 + Mathf.PerlinNoise(0.234f + xoff, 0.234f + yoff) * 7);

                        if (y < noise)
                            if ( y > noise - 5)
                                sChunk.set(x,y,z, block.Grass_block);
                            else
                                sChunk.set(x,y,z, block.Stone_block);
                    
                    }
                    yoff += 0.05f;
                }
                xoff += 0.05f;
            }

            updateWorld = false;
        }
    }
}
