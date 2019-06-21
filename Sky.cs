using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sky : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Mesh mesh;

    Color32 blueSky = new Color32(78,158,217,255);
    Color32 sunset = new Color32(255,117,119,255);
    Color32 sunrise = new Color32(255,220,186,255);


    public void StartSky()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();

        // Create the mesh once, at start
        mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    public void UpdateSky()
    {
        float x1 = GameCore.Core.landPointX[GameCore.Core.landSections-1];

        // Vertices

        Vector3[] verts = new Vector3[4];

        verts[0] = new Vector3(0f, 0f, 0f);
        verts[1] = new Vector3(0f, 200f, 0f);
        verts[2] = new Vector3(x1, 200f, 0f);
        verts[3] = new Vector3(x1, 0f, 0f);

        // Triangles

        int[] tris = new int[6]
        { 0, 1, 2,
          0, 2, 3 };
        
        // UVs

        Vector2[] uvs = new Vector2[4];

        uvs[0] = new Vector2(0,0);
        uvs[1] = new Vector2(0,1);
        uvs[2] = new Vector2(1,1);
        uvs[3] = new Vector2(1,0);

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;

        //mesh.RecalculateBounds(); <--- is this needed ?
        
        // color


        Color32 UpperColor = Color.red;
        Color32 LowerColor = Color.red;

        Color32 skyColor = Color.red;   // sky color based on time of day

        // set sky color

        float t;

        t = GameCore.Core.timeStamp - Mathf.FloorToInt(GameCore.Core.timeStamp);
        
        if (t < (3f/24f)) // 00:00 - 03:00
        {
            skyColor = Color.magenta;
        }
        else
        if (t < (9f/24f)) // 03:00 - 09:00
        {
            skyColor = Color.Lerp(sunrise, blueSky, (t-(3f/24f)) / (6f/24f));
        }
        else
        if (t < (18f/24f)) // 09:00 - 18:00
        {
            skyColor = blueSky;
        }
        else
        if (t < (21f/24f)) // 18:00 - 21:00
        {
            skyColor = Color.Lerp(blueSky, sunset, (t-(18f/24f)) / (3f/24f));
        }
        else               // 21:00 - 00:00
        {
            skyColor = Color.magenta;
        }
        

        // set gradient
        
        if (t < (6f/24f)) // 00:00 - 06:00
        {
            UpperColor = Color.black;
            LowerColor = Color.Lerp(Color.black, skyColor, (t-(0f/24f)) / (6f/24f));
        }
        else
        if (t < (12f/24f)) // 06:00 - 12:00
        {
            UpperColor = Color.Lerp(Color.black, skyColor, (t-(6f/24f)) / (6f/24f));
            LowerColor = Color.Lerp(skyColor, Color.white, (t-(6f/24f)) / (6f/24f));
        }
        else
        if (t < (18f/24f)) // 12:00 - 18:00
        {
            UpperColor = Color.Lerp(skyColor, Color.black, (t-(12f/24f)) / (6f/24f));
            LowerColor = Color.Lerp(Color.white, skyColor, (t-(12f/24f)) / (6f/24f));
        }
        else               // 18:00 - 00:00
        {
            UpperColor = Color.black;
            LowerColor = Color.Lerp(skyColor, Color.black, (t-(18f/24f)) / (6f/24f));
        }

        //

        Color[] colors = new Color[4];

        // apply color gradient
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            colors[i] = Color.Lerp(LowerColor, UpperColor, mesh.vertices[i].y); 
        }

        // assign the array of colors to the Mesh.
        mesh.colors = colors;

    }

    // ===================================================== main =====================================================

	void Start ()
    {
		StartSky();
	}
	
	void Update ()
    {
		UpdateSky();
	}
}