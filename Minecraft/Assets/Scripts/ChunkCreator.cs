using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChunkCreator
{
    const int CX = 16;
    const int CY = 16;
    const int CZ = 16;

    public byte[,,] block = new byte[CX, CY, CZ];
    public Vector3[] vertex = new Vector3[CX*CY*CZ*6*6];
    public int[] indices;

    Vector2[] uv = new Vector2[CX*CY*CZ*6*6];    

    // Texture 
    byte[,,]    top   = new byte[CX,CY,CZ],
                bot   = new byte[CX,CY,CZ],
                left  = new byte[CX,CY,CZ],
                right = new byte[CX,CY,CZ],
                front = new byte[CX,CY,CZ],
                back  = new byte[CX,CY,CZ],
                ID    = new byte[CX,CY,CZ];
    
    public bool changed;
    int elements;

    public ChunkCreator() {
        memsetByte(block, CX, CY, CZ);
        memsetVec2(uv);

        elements = 0;
        changed = true;
    }

    public byte get (int x, int y, int z) {

        return block[x,y,z];
    }

    public void setBlock(int x, int y, int z, block_type type) {
        block[x,y,z] = type.ID;

        // textures
        top[x,y,z]   = type.top;
        bot[x,y,z]   = type.bot;
        left[x,y,z]  = type.left;
        right[x,y,z] = type.right;
        front[x,y,z] = type.front;
        back[x,y,z]  = type.back;

        changed = true;
    }
    
    void update(bool[,,] externFaces) 
    {
        
        changed = false;

        int i = 0;
        int n = 0;

        // NEGATIVE X
        for (int x = 0; x < CX; x++)
        {
            for (int y = 0; y < CY; y++)
            {
                for (int z = 0; z < CZ; z++)
                {
                    //Empty block?
                    if(block[x,y,z] == 0)
                    {
                        continue;
                    }

                    //View from negative x (first face x = 0) 
                    if (x > 0 && block[x - 1,y,z] == 0) {

                        vertex[i++] = new Vector3 (x, y    , z + 1);   //vertex[0]
                        vertex[i++] = new Vector3 (x, y + 1, z + 1);   //vertex[1]
                        vertex[i++] = new Vector3 (x, y    , z    );   //...
                        vertex[i++] = new Vector3 (x, y    , z    );
                        vertex[i++] = new Vector3 (x, y + 1, z + 1);
                        vertex[i++] = new Vector3 (x, y + 1, z    );
                        //int this point i = 6 if x != 0
                        
                        uv[n++] = new Vector2(left[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(left[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((left[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((left[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(left[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((left[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                    else if (x == 0 && externFaces[0,y,z]) {

                        vertex[i++] = new Vector3 (x, y    , z + 1);   //vertex[0]
                        vertex[i++] = new Vector3 (x, y + 1, z + 1);   //vertex[1]
                        vertex[i++] = new Vector3 (x, y    , z    );   //...
                        vertex[i++] = new Vector3 (x, y    , z    );
                        vertex[i++] = new Vector3 (x, y + 1, z + 1);
                        vertex[i++] = new Vector3 (x, y + 1, z    );
                        // i = 6    

                        uv[n++] = new Vector2(left[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(left[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((left[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((left[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(left[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((left[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                }
            }
        }
        /*
        // Set texture UV for -x
        for (int n = 0; n < i; n = n + 6) {
            
            uv[n] =     new Vector2(0 / 16f, 0 / 23f);
            uv[n + 1] = new Vector2(0 / 16f, 1 / 23f);
            uv[n + 2] = new Vector2(1 / 16f, 0 / 23f);
            uv[n + 3] = new Vector2(1 / 16f, 0 / 23f);
            uv[n + 4] = new Vector2(0 / 16f, 1 / 23f);
            uv[n + 5] = new Vector2(1 / 16f, 1 / 23f); 
        }*/

        int texOffset = i;

        // POSITIVE X
        for (int x = 0; x < CX; x++)
        {
            for (int y = 0; y < CY; y++)
            {
                for (int z = 0; z < CZ; z++)
                {
                    //Empty block?
                    if(block[x,y,z] == 0)
                    {
                        continue;
                    }

                    if (x < 15 && block[x + 1,y,z] == 0) {

                        //View from positive x (second face in x = 1)
                        vertex[i++] = new Vector3 (x + 1, y    , z    );
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x + 1, y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);

                        uv[n++] = new Vector2(right[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(right[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((right[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((right[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(right[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((right[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                    
                    else if (x == 15 && externFaces[1,y,z]) {

                        //View from positive x (second face in x = 1)
                        vertex[i++] = new Vector3 (x + 1, y    , z    );
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x + 1, y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);

                        uv[n++] = new Vector2(right[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(right[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((right[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((right[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(right[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((right[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                }
            }
        }
        /*
        for (int l = texOffset; l < i; n = n + 6) {
            
            uv[n] =     new Vector2(vertex[n].z  ,0);
            uv[n + 1] = new Vector2(vertex[n+1].z,1);
            uv[n + 2] = new Vector2(vertex[n+2].z,0);
            uv[n + 3] = new Vector2(vertex[n+3].z,0);
            uv[n + 4] = new Vector2(vertex[n+4].z,1);
            uv[n + 5] = new Vector2(vertex[n+5].z,1); 
        }
        */
        texOffset = i;

        // NEGATIVE Y
        for (int x = 0; x < CX; x++)
        {
            for (int y = 0; y < CY; y++)
            {
                for (int z = 0; z < CZ; z++)
                {
                    //Empty block?
                    if(block[x,y,z] == 0)
                    {
                        continue;
                    }

                    if (y > 0 && block[x,y - 1,z] == 0) {

                        vertex[i++] = new Vector3 (x    , y, z + 1);
                        vertex[i++] = new Vector3 (x    , y, z    );
                        vertex[i++] = new Vector3 (x + 1, y, z + 1);
                        vertex[i++] = new Vector3 (x + 1, y, z + 1);
                        vertex[i++] = new Vector3 (x    , y, z    );
                        vertex[i++] = new Vector3 (x + 1, y, z    );

                        uv[n++] = new Vector2(bot[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(bot[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((bot[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((bot[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(bot[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((bot[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                    else if (y == 0 && externFaces[4,x,z]) {

                        vertex[i++] = new Vector3 (x    , y, z + 1);
                        vertex[i++] = new Vector3 (x    , y, z    );
                        vertex[i++] = new Vector3 (x + 1, y, z + 1);
                        vertex[i++] = new Vector3 (x + 1, y, z + 1);
                        vertex[i++] = new Vector3 (x    , y, z    );
                        vertex[i++] = new Vector3 (x + 1, y, z    );
                        
                        uv[n++] = new Vector2(bot[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(bot[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((bot[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((bot[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(bot[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((bot[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                }
            }
        }
        /*
        for (int n = texOffset; n < i; n = n + 6) {
            
            uv[n] =     new Vector2(vertex[n].z  ,0);
            uv[n + 1] = new Vector2(vertex[n+1].z,1);
            uv[n + 2] = new Vector2(vertex[n+2].z,0);
            uv[n + 3] = new Vector2(vertex[n+3].z,0);
            uv[n + 4] = new Vector2(vertex[n+4].z,1);
            uv[n + 5] = new Vector2(vertex[n+5].z,1); 
        }
        */

        texOffset = i;

        //POSITIVE Y
        for (int x = 0; x < CX; x++)
        {
            for (int y = 0; y < CY; y++)
            {
                for (int z = 0; z < CZ; z++)
                {
                    //Empty block?
                    if(block[x,y,z] == 0)
                    {
                        continue;
                    }
                    
                    if (y < 15 && block[x,y + 1,z] == 0) {

                        vertex[i++] = new Vector3 (x    , y + 1, z    );
                        vertex[i++] = new Vector3 (x    , y + 1, z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x    , y + 1, z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);
                        
                        uv[n++] = new Vector2(top[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(top[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((top[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((top[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(top[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((top[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                    else if (y == 15 && externFaces[5, x,z]) {

                        vertex[i++] = new Vector3 (x    , y + 1, z    );
                        vertex[i++] = new Vector3 (x    , y + 1, z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x + 1, y + 1, z    );
                        vertex[i++] = new Vector3 (x    , y + 1, z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);

                        uv[n++] = new Vector2(top[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(top[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((top[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((top[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(top[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((top[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                }
            }
        }
        /*
        for (int n = texOffset; n < i; n = n + 6) {
            
            uv[n] =     new Vector2(vertex[n].z  ,0);
            uv[n + 1] = new Vector2(vertex[n+1].z,1);
            uv[n + 2] = new Vector2(vertex[n+2].z,0);
            uv[n + 3] = new Vector2(vertex[n+3].z,0);
            uv[n + 4] = new Vector2(vertex[n+4].z,1);
            uv[n + 5] = new Vector2(vertex[n+5].z,1); 
        }
        */
        texOffset = i;

        //NEGATIVE Z
        for (int z = 0; z < CZ; z++)
        {
            for (int y = 0; y < CY; y++)
            {
                for (int x = 0; x < CX; x++)
                {
                    //Empty block?
                    if(block[x,y,z] == 0)
                    {
                        continue;
                    }

                    if (z > 0 && block[x,y,z - 1] == 0) {

                        vertex[i++] = new Vector3 (x    , y    , z);
                        vertex[i++] = new Vector3 (x    , y + 1, z);
                        vertex[i++] = new Vector3 (x + 1, y    , z);
                        vertex[i++] = new Vector3 (x + 1, y    , z);
                        vertex[i++] = new Vector3 (x    , y + 1, z);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z);
                        
                        uv[n++] = new Vector2(front[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(front[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((front[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((front[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(front[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((front[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                    else if (z == 0 && externFaces[2,x,y]) {

                        vertex[i++] = new Vector3 (x    , y    , z);
                        vertex[i++] = new Vector3 (x    , y + 1, z);
                        vertex[i++] = new Vector3 (x + 1, y    , z);
                        vertex[i++] = new Vector3 (x + 1, y    , z);
                        vertex[i++] = new Vector3 (x    , y + 1, z);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z);

                        uv[n++] = new Vector2(front[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(front[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((front[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((front[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(front[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((front[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                }
            }
        }
        /*
        for (int n = texOffset; n < i; n = n + 6) {
            
            uv[n] = new Vector2(vertex[n].x,0);
            uv[n + 1] = new Vector2(vertex[n+1].x,1);
            uv[n + 2] = new Vector2(vertex[n+2].x,0);
            uv[n + 3] = new Vector2(vertex[n+3].x,0);
            uv[n + 4] = new Vector2(vertex[n+4].x,1);
            uv[n + 5] = new Vector2(vertex[n+5].x,1); 
        }
        */
        texOffset = i;

        //POSITIVE Z
        for (int z = 0; z < CZ; z++)
        {
            for (int y = 0; y < CY; y++)
            {
                for (int x = 0; x < CX; x++)
                {
                    //Empty block?
                    if(block[x,y,z] == 0)
                    {
                        continue;
                    }

                    
                    if (z < 15 && block[x,y,z + 1] == 0) {

                        vertex[i++] = new Vector3 (x + 1, y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);
                        vertex[i++] = new Vector3 (x    , y    , z + 1);
                        vertex[i++] = new Vector3 (x    , y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);
                        vertex[i++] = new Vector3 (x    , y + 1, z + 1);

                        uv[n++] = new Vector2(back[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(back[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((back[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((back[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(back[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((back[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                    else if (z == 15 && externFaces[3,x,y]) {

                        vertex[i++] = new Vector3 (x + 1, y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);
                        vertex[i++] = new Vector3 (x    , y    , z + 1);
                        vertex[i++] = new Vector3 (x    , y    , z + 1);
                        vertex[i++] = new Vector3 (x + 1, y + 1, z + 1);
                        vertex[i++] = new Vector3 (x    , y + 1, z + 1);

                        uv[n++] = new Vector2(back[x,y,z] / 16f, 22 / 23f);
                        uv[n++] = new Vector2(back[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((back[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2((back[x,y,z] + 1)/ 16f, 22 / 23f);
                        uv[n++] = new Vector2(back[x,y,z] / 16f, 23 / 23f);
                        uv[n++] = new Vector2((back[x,y,z] + 1)/ 16f, 23 / 23f); 
                    }
                }
            }
        }   
        /*
        for (int n = texOffset; n < i; n = n + 6) {
            
            uv[n] = new Vector2(vertex[n].x,0);
            uv[n + 1] = new Vector2(vertex[n + 1].x,1);
            uv[n + 2] = new Vector2(vertex[n + 2].x,0);
            uv[n + 3] = new Vector2(vertex[n + 3].x,0);
            uv[n + 4] = new Vector2(vertex[n + 4].x,1);
            uv[n + 5] = new Vector2(vertex[n + 5].x,1); 
        }
        */
        elements = i;


        // set the indices
        indices = new int[elements];

        // just indices (Vertex) that we need
        for (int nEle = 0; nEle < elements; nEle++)
            indices[nEle] = nEle;

        //Debug.Log("Numero Vertici: " + indices.Length);

    }
    
    public void render(Mesh mesh, bool[,,] externFaces) 
    {
        if (changed) {

            update(externFaces);
            mesh.Clear();

            mesh.vertices = vertex;
            mesh.triangles = indices;
            mesh.uv = uv;
    
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
        }
    }

    void memsetByte(byte[,,] matrix, int cx, int cy, int cz)
    {
        for (int x=0; x<cx; x++)
            for (int y=0; y<cy; y++)
                for (int z=0; z<cz; z++)
                    matrix[x,y,z] = 0;
    }
    
    void memsetVec2 (Vector2[] array) {
        for (int i = 0; i<array.Length; i++)
            array[i] = Vector2.zero;
    }
}