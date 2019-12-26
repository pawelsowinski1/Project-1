using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackRectangle : MonoBehaviour
{
    public Mesh mesh;
    public MeshFilter meshFilter;

    public List<Vector3> verts = null;
    //public List<int> tris = null;

    public bool isItLeftRectangle;

    public void Draw()
    {
        GameCore core = GameCore.Core;

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        verts.Clear();
        //tris.Clear();

        if (isItLeftRectangle)
        {
            verts.Add(new Vector3(core.landPointX[0], core.landPointY[0]-1f, 0));
            verts.Add(new Vector3(core.landPointX[0], core.landPointY[0]+1f, 0));
            verts.Add(new Vector3(core.landPointX[0]-1f, core.landPointY[0]+1f, 0));
            verts.Add(new Vector3(core.landPointX[0]-1f, core.landPointY[0]-1f, 0));
        }
        else
        {
            verts.Add(new Vector3(core.landPointX[0], core.landPointY[0]-1f, 0));
            verts.Add(new Vector3(core.landPointX[0], core.landPointY[0]+1f, 0));
            verts.Add(new Vector3(core.landPointX[0]+1f, core.landPointY[0]+1f, 0));
            verts.Add(new Vector3(core.landPointX[0]+1f, core.landPointY[0]-1f, 0));

            transform.position = new Vector3(core.landPointX[core.landSections-1],0f,0f);
        }

        int[] tris = new int[6]
        { 0, 1, 2,
            0, 2, 3 };

        mesh.SetVertices(verts);
        mesh.triangles = tris;

    }

    void Start()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        
        // Create the mesh once, at start
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        Draw();
        
    }

}
