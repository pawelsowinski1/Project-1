using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCore : MonoBehaviour
{
    public Mesh mesh;
    public int index;

    int i;

    public List<Vector3> verts = null;
    public List<int> tris = null;


    public void Draw()
    {
        GameCore core = GameCore.Core;

        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        verts.Clear();
        tris.Clear();
        
        for (i=0; i<core.landSections-1; i++)
        {
            if (core.landPointY[i+1] > core.landPointY[i])
            {
                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i+1], 0));

                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i]-100f, 0));

                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i]-100f, 0));
                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i]-100f, 0));
            }
            else
            {
                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i+1], 0));
                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i+1], 0));

                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i+1], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i+1], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i]-100f, 0));

                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i+1], 0));
                verts.Add(new Vector3(core.landPointX[i+1], core.landPointY[i]-100f, 0));
                verts.Add(new Vector3(core.landPointX[i], core.landPointY[i]-100f, 0));
            }

            tris.Add(i*9);
            tris.Add(i*9+1);
            tris.Add(i*9+2);

            tris.Add(i*9+3);
            tris.Add(i*9+4);
            tris.Add(i*9+5);

            tris.Add(i*9+6);
            tris.Add(i*9+7);
            tris.Add(i*9+8);
/*
            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));*/
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        //mesh.SetUVs(0,uvs);

        mesh.RecalculateBounds();


        // color

        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        Color c = new Color(1f,1f,1f,1f);


        if (index == 0)
        c = new Color(0.5f,0.5f,0.5f,1f);
        else
        if (index == 1)
        c = new Color(0.7f,0.7f,0.7f,1f);
        else
        if (index == 2)
        c = new Color(0.9f,0.9f,0.9f,1f);
        else
        if (index == 3)
        c = new Color(1f,1f,1f,1f);

        for (int i = 0; i < vertices.Length; i++)
        colors[i] = c;

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
        

    }

	void Start ()
    {
		Draw();
	}
	
	void Update ()
    {
        if (index == 1)
		transform.position = new Vector3(Camera.main.transform.position.x * 0.50f - 200f, -Camera.main.transform.position.y * 0.1f + 5f, transform.position.z);
        else
        if (index == 2)
		transform.position = new Vector3(Camera.main.transform.position.x * 0.65f + 300f, -Camera.main.transform.position.y * 0.09f * 0.1f + 10f, transform.position.z);
        else
        if (index == 3)
		transform.position = new Vector3(Camera.main.transform.position.x * 0.80f - 200f, -Camera.main.transform.position.y * 0.08f * 0.05f + 15f, transform.position.z);

	}
}
