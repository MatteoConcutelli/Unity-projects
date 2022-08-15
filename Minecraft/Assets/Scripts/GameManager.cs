using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Player;
    public Material ChunkMaterial;
    Blocks block = new Blocks();
    ChunkManager sChunk;
    public Camera cam;
    World world;

    // Start is called before the first frame update
    void Start()
    {    
        sChunk = new ChunkManager(ChunkMaterial);
        world = new World(Player.transform.position);

    }

    // Update is called once per frame
    void Update()
    {
        world.update(sChunk, Player.transform.position);
        mouseInput();
        sChunk.render();
        
    }

    void mouseInput() 
    {
        if (Input.GetMouseButtonDown(1)) 
            SetBlock();
        else if (Input.GetMouseButtonDown(0)) 
            removeBlock();
    }

    void SetBlock() 
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int x = 0, y = 0, z = 0;

        if (Physics.Raycast(ray, out hit, 5)) {
            Debug.Log(hit.point);
        }

        x = (int) hit.point.x;
        y = (int) hit.point.y;
        z = (int) hit.point.z;

        // Get triangle index
        MeshCollider meshCollider = hit.collider as MeshCollider;
        if (meshCollider == null || meshCollider.sharedMesh == null) {
            Debug.Log("is returned");
            return;
        }
              
        Mesh mesh = meshCollider.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        // triangle vertices
        Vector3 p0 = vertices[triangles[hit.triangleIndex * 3 + 0]];
        Vector3 p1 = vertices[triangles[hit.triangleIndex * 3 + 1]];
        Vector3 p2 = vertices[triangles[hit.triangleIndex * 3 + 2]];
        Debug.Log(p0);
        Debug.Log(p1);
        Debug.Log(p2);

        // knowing the face from where it sees
        // +x && -x
        if (p0.x == p1.x && p0.x == p2.x) {
            if (cam.transform.position.x < hit.point.x) {
                x =(int)((hit.point.x - p0.x) + p0.x) - 1;
            }
            else {
                x =(int)((hit.point.x - p0.x) + p0.x);
            }
        }
        // +y && -y
        else if (p0.y == p1.y && p0.y == p2.y) {
            if (cam.transform.position.y < hit.point.y) {
                y =(int)((hit.point.y - p0.y) + p0.y) - 1;
            }
            else {
                y =(int)((hit.point.y - p0.y) + p0.y);
            }
        }
        // +z && -z
        else if (p0.z == p1.z && p0.z == p2.z) {
            if (cam.transform.position.z < hit.point.z) {
                z =(int)((hit.point.z - p0.z) + p0.z) - 1;
            }
            else {
                z =(int)((hit.point.z - p0.z) + p0.z);
            }
        }

        Debug.Log("coord: " + new Vector3 (x,y,z));
        sChunk.set(x, y, z, block.Wood_block);

    }   

    void removeBlock () 
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int x = 0, y = 0, z = 0;

        if (Physics.Raycast(ray, out hit, 5)) {

            x = (int) hit.point.x;
            y = (int) hit.point.y;
            z = (int) hit.point.z;

            if (cam.transform.position.x > hit.point.x && hit.point.x % 1 == 0)
                x -= 1;
            else if (cam.transform.position.y > hit.point.y && hit.point.y % 1 == 0)
                y -= 1;
            else if (cam.transform.position.z > hit.point.z && hit.point.z % 1 == 0)
                z -= 1;
            
            sChunk.set(x, y, z, block.Air_block);
        }
    }
}
