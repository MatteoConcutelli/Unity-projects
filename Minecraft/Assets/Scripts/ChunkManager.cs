using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {
    
    // Chunk data
    ChunkCreator[,,] c = new ChunkCreator[16,16,16];
    // Chunk GameObject
    GameObject[,,] Chunk = new GameObject[16,16,16];
    Mesh[,,] mesh = new Mesh [16,16,16];
    Renderer[,,] rend = new Renderer[16,16,16];
    Material ChunkMaterial;

    // check if the chunk is changed
    bool[,,] changed = new bool[16,16,16];

    bool[,,] externFaces = new bool[16,16,16];
    /* x is the face
        0 -> x0
        1 -> x15
        2 -> z0
        3 -> z15
        4 -> y0
        5 -> y15
    */

    public ChunkManager(Material material) 
    {
        for (int x = 0; x < 16; x++)
            for (int y = 0; y < 16; y++) {
                for( int z = 0; z < 16; z++) {
                    c[x,y,z] = null;
                }
            }
        
        ChunkMaterial = material;
        
    }

    public byte get (int x, int y, int z) {

        int cx = x/16;
        int cy = y/16;
        int cz = z/16;

        x %= 16;
        y %= 16;
        z %= 16;

        if (c[cx,cy,cz] == null )
            return 0;
        else
            return c[cx,cy,cz].get(x, y, z);
        
    }
    
    public void set(int x, int y, int z, block_type blockType) {
        
        int cx = x/16;
        int cy = y/16;
        int cz = z/16;

        x %= 16;
        y %= 16;
        z %= 16;

        if (c[cx,cy,cz] == null) {
            c[cx,cy,cz] = new ChunkCreator();
            createChunkObject (cx, cy, cz);
        }

        c[cx,cy,cz].setBlock(x, y, z, blockType);
        
        // update the chunk near
        // if the block is setted in 0 
        updateTheChunkNear(x,y,z,cx,cy,cz);
        
        changed[cx,cy,cz] = true;
    }

    public void render() 
    {
        for (int x = 0; x < 16; x++) {
            for (int y = 0; y < 16; y++) {
                for( int z = 0; z < 16; z++) {
                    if (c[x,y,z] != null) {
                        // translate chunk when is created 
                        Chunk[x,y,z].transform.position = new Vector3(x * 16, y*16, z * 16);
                        
                        deleteExternFaces(x,y,z);
                        c[x,y,z].render(mesh[x,y,z], externFaces);

                        // Update the Mesh Collider of the Chunk when he changes
                        if (Chunk[x,y,z].GetComponent<MeshCollider>() && changed[x,y,z]) {
                                Destroy(Chunk[x,y,z].GetComponent<MeshCollider>());
                                Chunk[x,y,z].AddComponent<MeshCollider>();
                                changed[x,y,z] = false;
                        }
                    }
                }
            }
        }
    }
    
    void createChunkObject (int cx, int cy, int cz) 
    {
        Chunk[cx,cy,cz] = new GameObject("chunk");
        Chunk[cx,cy,cz].AddComponent<MeshFilter>();
        Chunk[cx,cy,cz].AddComponent<MeshRenderer>();
        Chunk[cx,cy,cz].GetComponent<Renderer>().material = ChunkMaterial;

        rend[cx,cy,cz] = Chunk[cx,cy,cz].GetComponent<Renderer>();
    
        mesh[cx,cy,cz] = new Mesh();
        Chunk[cx,cy,cz].GetComponent<MeshFilter>().mesh = mesh[cx,cy,cz];
        Chunk[cx,cy,cz].AddComponent<MeshCollider>();
    }
    
    void updateTheChunkNear(int x, int y, int z, int cx, int cy, int cz) 
    {
        // x0 (when the block in x = 15 is setted = 0, 
        // the chunk near in x + 1 will update the x = 0 face) 
        if (x == 15 && cx !=  15 && c[cx + 1, cy, cz] != null) { 
            c[cx + 1, cy, cz].changed = true;
            changed[cx + 1,cy,cz] = true;
        }

        // x15
        if (x == 0 && cx != 0 && c[cx - 1, cy, cz] != null) {
            c[cx - 1, cy, cz].changed = true;
            changed[cx - 1,cy,cz] = true;
        }
        
        // z0
        if (z == 15 && cz != 15 && c[cx,cy,cz + 1] != null) {
            c[cx,cy,cz + 1].changed = true;
            changed[cx,cy,cz + 1] = true;
        }

        // z15
        if (z == 0 && cz != 0 && c[cx,cy,cz - 1] != null) {
            c[cx, cy, cz - 1].changed = true;
            changed[cx,cy,cz - 1] = true;
        }
        
        // y0
        if (y == 15 && c[cx, cy + 1, cz] != null) {
            c[cx,cy + 1,cz].changed = true;
            changed[cx,cy + 1,cz] = true;
        }
        // y15
        if (y == 0 && cy != 0 && c[cx, cy - 1, cz] != null) {
            c[cx, cy - 1, cz].changed = true;
            changed[cx,cy - 1,cz] = true;
        }
    }


    void deleteExternFaces (int x, int y, int z)
    {
        // check for x0 faces
        if (x > 0 && c[x - 1,y,z] != null) {
            for (int y0 = 0; y0 < 16; y0++) {
                for (int z0 = 0; z0 < 16; z0++) {  
                    if (c[x - 1, y, z].block[15, y0,z0] == 0) 
                        externFaces[0,y0,z0] = true;
                    else 
                        externFaces[0,y0, z0] = false;
                }
            }
        }
        else if (x > 0 && c[x - 1,y,z] == null) 
            for (int y0 = 0; y0 < 16; y0++) 
                for (int z0 = 0; z0 < 16; z0++) 
                    externFaces[0,y0,z0] = true;
        else 
            for (int y0 = 0; y0 < 16; y0++)
                for (int z0 = 0; z0 < 16; z0++)
                    externFaces[0,y0, z0] = false;


        // check for x15 faces
        if(x < 15 && c[x + 1, y, z] != null) {
           for (int y15 = 0; y15 < 16; y15++) {
                for (int z15 = 0; z15 < 16; z15++) {
                    if (c[x + 1, y, z].block[0, y15, z15] == 0)
                        externFaces[1,y15,z15] = true;
                    else 
                        externFaces[1,y15,z15] = false;
                }
            }   
        }
        else if (x < 15 && c[x + 1, y, z] == null)
            for (int y15 = 0; y15 < 16; y15++) 
                for (int z15 = 0; z15 < 16; z15++) 
                    externFaces[1,y15, z15] = true;
        else 
            for (int y15 = 0; y15 < 16; y15++) 
                for (int z15 = 0; z15 < 16; z15++) 
                    externFaces[1,y15, z15] = false;


        // check for z0 faces
        if (z > 0 && c[x,y,z - 1] != null) {
            for (int x0 = 0; x0 < 16; x0++) {
                for (int y0 = 0; y0 < 16; y0++) {  
                    if (c[x, y, z - 1].block[x0, y0, 15] == 0) 
                        externFaces[2,x0,y0] = true;
                    else 
                        externFaces[2,x0, y0] = false;
                }
            }
        }
        else if (z > 0 && c[x,y,z - 1] == null)
            for (int x0 = 0; x0 < 16; x0++) 
                for (int y0 = 0; y0 < 16; y0++) 
                    externFaces[2,x0,y0] = true;
        else 
            for (int x0 = 0; x0 < 16; x0++) 
                for (int y0 = 0; y0 < 16; y0++) 
                    externFaces[2,x0,y0] = false;


        // check for z15 faces
        if (z < 15 && c[x,y,z + 1] != null) {
            for (int x15 = 0; x15 < 16; x15++) {
                for (int y15 = 0; y15 < 16; y15++) {
                    if (c[x,y,z+1].block[x15,y15,0] == 0) {
                        externFaces[3,x15,y15] = true;
                    }
                    else 
                        externFaces[3,x15,y15] = false;
                }
            }
        }
        else if (z < 15 && c[x,y,z + 1] == null)
            for (int x15 = 0; x15 < 16; x15++) 
                for (int y15 = 0; y15 < 16; y15++) 
                    externFaces[3,x15,y15] = true;
        else 
            for (int x15 = 0; x15 < 16; x15++) 
                for (int y15 = 0; y15 < 16; y15++) 
                    externFaces[3,x15,y15] = false;
        

        // check for y0 faces
        if (y > 0 && c[x, y - 1, z] != null) {
            for (int x0 = 0; x0 < 16; x0++) {
                for (int z0 = 0; z0 < 16; z0++) {
                    if (c[x, y - 1, z].block[x0, 15, z0] == 0) 
                        externFaces[4,x0, z0] = true;
                    else 
                        externFaces[4,x0, z0] = false;
                }
            }
        }
        else
            for (int x0 = 0; x0 < 16; x0++)
                for (int z0 = 0; z0 < 16; z0++) 
                    externFaces[4,x0, z0] = true;
                
        // check for y15 faces
        if (y < 15 && c[x,y + 1,z] != null) {
            for (int x15 = 0; x15 < 16; x15++) {
                for (int z15 = 0; z15 < 16; z15++) {
                    if (c[x,y + 1, z].block[x15, 0, z15] == 0)
                        externFaces[5,x15, z15] = true;
                    else 
                        externFaces[5,x15,z15] = false;
                }
            }
        }
        else 
            for (int x15 = 0; x15 < 16; x15++) 
                for (int z15 = 0; z15 < 16; z15++)
                    externFaces[5,x15, z15] = true;
    }
    
}