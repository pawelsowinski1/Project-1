using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Sky : MonoBehaviour
{
    public MeshFilter meshFilter;
    public Mesh mesh;

    Color32 blue = new Color32(78,158,217,255);
    Color32 brightBlue = new Color32(177,210,235,255);
    Color32 darkBlue = new Color32(32,58,77,255);
    Color32 sunset = new Color32(255,68,71,255);
    Color32 sunrise = new Color32(255,166,84,255);


    public void StartSky()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();

        // Create the mesh once, at start
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        // find lowest point of the ground

        float f = 10000f;

        for (int i=0; i<GameCore.Core.landSections; i++)
        {
            if (GameCore.Core.landPointY[i] < f)
            {
                f = GameCore.Core.landPointY[i];
            }
        }

        // move the sky, so that its bottom edge is in the lowest point of the ground

        transform.position = new Vector3(transform.position.x,f+0f,transform.position.z);
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

        Color32 skyColor = Color.red;   // current sky color based on time of day

        // set base sky color

        float t;

        t = GameCore.Core.timeStamp - Mathf.FloorToInt(GameCore.Core.timeStamp);
        
        if (t < (4f/24f)) // 00:00 - 04:00
        {
            skyColor = Color.Lerp(Color.black, sunrise, (t-(0f/24f)) / (4f/24f));
        }
        else
        if (t < (7f/24f)) // 04:00 - 07:00
        {
            skyColor = Color.Lerp(sunrise, blue, (t-(4f/24f)) / (3f/24f));
        }
        else
        if (t < (18f/24f)) // 07:00 - 18:00
        {
            skyColor = blue;
        }
        else
        if (t < (21f/24f)) // 18:00 - 21:00
        {
            skyColor = Color.Lerp(blue, sunset, (t-(18f/24f)) / (3f/24f));
        }
        else               // 21:00 - 00:00
        {
            skyColor = Color.Lerp(sunset, Color.black, (t-(21f/24f)) / (3f/24f));
        }
        

        // set sky color gradient according to time
        
        if (t < (2f/24f)) // 00:00 - 02:00
        {
            UpperColor = Color.black;
            LowerColor = Color.black;
        }
        else
        if (t < (8f/24f)) // 02:00 - 08:00
        {
            UpperColor = Color.Lerp(Color.black, blue, (t-(2f/24f)) / (6f/24f));
            LowerColor = Color.Lerp(Color.black, skyColor, (t-(2f/24f)) / (6f/24f));
        }
        else
        if (t < (12f/24f)) // 08:00 - 12:00
        {
            UpperColor = Color.Lerp(blue, skyColor, (t-(8f/24f)) / (4f/24f));
            LowerColor = Color.Lerp(skyColor, brightBlue, (t-(8f/24f)) / (4f/24f));
        }
        else
        if (t < (18f/24f)) // 12:00 - 18:00
        {
            UpperColor = Color.Lerp(skyColor, blue, (t-(12f/24f)) / (6f/24f));
            LowerColor = Color.Lerp(brightBlue, skyColor, (t-(12f/24f)) / (6f/24f));
        }
        else
        if (t < (21f/24f)) // 18:00 - 21:00
        {
            UpperColor = Color.Lerp(blue, darkBlue, (t-(18f/24f)) / (3f/24f));
            LowerColor = Color.Lerp(skyColor, Color.black, (t-(18f/24f)) / (6f/24f));
        }
        else               // 21:00 - 00:00
        {
            UpperColor = Color.Lerp(darkBlue, Color.black, (t-(21f/24f)) / (3f/24f));
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