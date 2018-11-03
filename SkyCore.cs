using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkyCore : MonoBehaviour
{

    public Mesh mesh;

    public List<Vector3> verts = null;
    public List<int> tris = null;
    public List<Vector2> uvs = null;

    int i;


    void DrawSky()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();

        float x1;

        x1 = GameCore.Core.landPointX[GameCore.Core.landSections-1];

        verts.Add(new Vector3(0f, 0f, 0f));
        verts.Add(new Vector3(0f, 200f, 0f));
        verts.Add(new Vector3(x1, 200f, 0f));

        verts.Add(new Vector3(0f, 0f, 0f));
        verts.Add(new Vector3(x1, 200f, 0f));
        verts.Add(new Vector3(x1, 0f, 0f));

        tris.Add(0);
        tris.Add(1);
        tris.Add(2);

        tris.Add(3);
        tris.Add(4);
        tris.Add(5);

        uvs.Add(new Vector2(1,1));
        uvs.Add(new Vector2(0,0));
        uvs.Add(new Vector2(0,0));

        uvs.Add(new Vector2(1,1));
        uvs.Add(new Vector2(0,0));
        uvs.Add(new Vector2(1,1));

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        mesh.SetUVs(0,uvs);

        mesh.RecalculateBounds();

        
        // color

        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        colors[i] = Color.Lerp(Color.white, Color.black, vertices[i].y);

        // assign the array of colors to the Mesh.
        mesh.colors = colors;
        


        /*
        
        for (i=0; i<landSections-1; i++)
        {
            if (landPointY[i+1] > landPointY[i])
            {
                verts.Add(new Vector3(landPointX[i], landPointY[i], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i+1], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i], 0));

                verts.Add(new Vector3(landPointX[i], landPointY[i], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i]-100f, 0));

                verts.Add(new Vector3(landPointX[i], landPointY[i], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i]-100f, 0));
                verts.Add(new Vector3(landPointX[i], landPointY[i]-100f, 0));
            }
            else
            {
                verts.Add(new Vector3(landPointX[i], landPointY[i], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i+1], 0));
                verts.Add(new Vector3(landPointX[i], landPointY[i+1], 0));

                verts.Add(new Vector3(landPointX[i], landPointY[i+1], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i+1], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i]-100f, 0));

                verts.Add(new Vector3(landPointX[i], landPointY[i+1], 0));
                verts.Add(new Vector3(landPointX[i+1], landPointY[i]-100f, 0));
                verts.Add(new Vector3(landPointX[i], landPointY[i]-100f, 0));
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
        /*
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        //mesh.SetUVs(0,uvs);

        mesh.RecalculateBounds();
        */
    //}


	void Start ()
    {
		DrawSky();

	}
	
	void Update ()
    {
		
	}
}
