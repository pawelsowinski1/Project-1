using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WorldMapCore : MonoBehaviour
{
    GameObject clone;
    GameObject clone2;

    public GameObject buttonWPrefab;
    public GameObject imagePrefab;

    public List<GameObject> worldMapObjects = new List<GameObject>();

    int i;

	public void DrawMap()
    {
        // --------------- nodes -----------------

        GameCore.Node n;
        float nx,ny;

        for (i=0; i<=GameCore.Core.nodes.Count-1; i++)
        {
            clone = Instantiate(buttonWPrefab, transform.position, transform.rotation) as GameObject;
            worldMapObjects.Add(clone);

            clone.transform.SetParent(transform,false);
            clone.transform.position = transform.position;
            clone.transform.localScale /= 2;

            if (GameCore.Core.nodes[i].visited == false)
            clone.GetComponent<Image>().color = Color.black;
            else
            clone.GetComponent<Image>().color = Color.gray;

            n = GameCore.Core.nodes[i];

            clone.GetComponentInChildren<Text>().text = "("+ n.a.ToString() +","+ n.b.ToString() + ")";

            // translate along "A" axis

            nx = -100f*n.a; // float * int => float
            ny = 50f*n.a;   // float * int => float
            clone.transform.position += new Vector3(nx,ny,0);

            // translate along "B" axis

            nx = 100f*n.b; // float * int => float
            ny = 50f*n.b;  // float * int => float
            clone.transform.position += new Vector3(nx,ny,0);
        }

        // ------------------- lands --------------------

        GameCore.Land l;

        for (i=0; i<=GameCore.Core.lands.Count-1; i++)
        {
            clone = Instantiate(buttonWPrefab, transform.position, transform.rotation) as GameObject;
            worldMapObjects.Add(clone);

            clone.GetComponent<ButtonWCore>().isLand =  true;
            clone.GetComponent<ButtonWCore>().landIndex = i;
            clone.GetComponent<ButtonWCore>().Colorize();

            clone.transform.SetParent(transform,false);
            clone.transform.position = transform.position;
            clone.GetComponentInChildren<Text>().text = i.ToString();

            

            l = GameCore.Core.lands[i];

            // translate along "A" axis

            nx = -100f * ((1f* l.nodeL.a + 1f* l.nodeR.a) / 2); // float * int => float
            ny = 50f * ((1f* l.nodeL.a + 1f* l.nodeR.a) / 2);   // float * int => float
            clone.transform.position += new Vector3(nx,ny,0);

            // translate along "B" axis

            nx = 100f * ((1f* l.nodeL.b + 1f* l.nodeR.b) / 2); // float * int => float
            ny = 50f * ((1f* l.nodeL.b + 1f* l.nodeR.b) / 2);  // float * int => float
            clone.transform.position += new Vector3(nx,ny,0);

        }

        // ---------------- units -----------------
        
        for (i=0; i<GameCore.Core.teams.Count; i++)
        {
            int j;

            for (j=0; j<GameCore.Core.teams[i].units.Count; j++)
            {
                if ((i == 1)
                && (j == 0))
                {
                    // player unit -> do not draw
                }
                else
                {
                    // draw unit on the world map

                    clone = Instantiate(imagePrefab, transform.position, transform.rotation) as GameObject;
                    worldMapObjects.Add(clone);

                    clone.transform.SetParent(transform,false);
                    clone.transform.position = transform.position;
                    clone.GetComponent<Image>().sprite = GameCore.Core.spr_unit;
                    clone.transform.localScale = new Vector3(0.35f,0.35f,0.35f);

                    clone.GetComponent<Image>().color = GameCore.Core.teams[i].color;

                    l = GameCore.Core.lands[GameCore.Core.teams[i].units[j].land];
                
                    //if (GameCore.Core.travelMode == false)
                    {
                        // translate along "A" axis

                        nx = -100f * ((1f* l.nodeL.a + 1f* l.nodeR.a) / 2); // float * int => float
                        ny = 50f * ((1f* l.nodeL.a + 1f* l.nodeR.a) / 2);   // float * int => float
                        clone.transform.position += new Vector3(nx,ny,0);

                        // translate along "B" axis

                        nx = 100f * ((1f* l.nodeL.b + 1f* l.nodeR.b) / 2); // float * int => float
                        ny = 50f * ((1f* l.nodeL.b + 1f* l.nodeR.b) / 2);  // float * int => float
                        clone.transform.position += new Vector3(nx,ny,0);
                    }

                    //
                }
                
            }
        }

        // ------------- player --------------

        clone = Instantiate(imagePrefab, transform.position, transform.rotation) as GameObject;
        worldMapObjects.Add(clone);

        clone.transform.SetParent(transform,false);
        clone.transform.position = transform.position;
        clone.GetComponent<Image>().sprite = GameCore.Core.spr_unit;
        clone.transform.localScale = new Vector3(0.35f,0.35f,0.35f);

        clone.GetComponent<Image>().color = GameCore.Core.teams[1].color;


        l = GameCore.Core.lands[GameCore.Core.currentLand];

        if (GameCore.Core.travelMode == false)
        {
            // translate along "A" axis

            nx = -100f * ((1f* l.nodeL.a + 1f* l.nodeR.a) / 2); // float * int => float
            ny = 50f * ((1f* l.nodeL.a + 1f* l.nodeR.a) / 2);   // float * int => float
            clone.transform.position += new Vector3(nx,ny,0);

            // translate along "B" axis

            nx = 100f * ((1f* l.nodeL.b + 1f* l.nodeR.b) / 2); // float * int => float
            ny = 50f * ((1f* l.nodeL.b + 1f* l.nodeR.b) / 2);  // float * int => float
            clone.transform.position += new Vector3(nx,ny,0);
        }
        else
        {
            if (GameCore.Core.travelRight == false) // player travelling left -> figure standing on left node
            {
                // translate along "A" axis
                nx = -100f * l.nodeL.a; // float * int => float
                ny = 50f * l.nodeL.a; // float * int => float
                clone.transform.position += new Vector3(nx,ny,0);

                // translate along "B" axis
                nx = 100f * l.nodeL.b; // float * int => float
                ny = 50f * l.nodeL.b; // float * int => float
                clone.transform.position += new Vector3(nx,ny,0);
            }
            else                                    // player travelling right -> figure standing on right node
            {
                // translate along "A" axis
                nx = -100f * l.nodeR.a; // float * int => float
                ny = 50f * l.nodeR.a; // float * int => float
                clone.transform.position += new Vector3(nx,ny,0);

                // translate along "B" axis
                nx = 100f * l.nodeR.b; // float * int => float
                ny = 50f * l.nodeR.b; // float * int => float
                clone.transform.position += new Vector3(nx,ny,0);
            }
        
        }


        // -----------------------------------------------


	}

    public void ClearMap()
    {
        for (i=0; i<worldMapObjects.Count; i++)
        {
            Destroy(worldMapObjects[i]);
        }

        worldMapObjects.Clear();
    }

    public void RedrawMap()
    {
        ClearMap();
        DrawMap();
    }

    // ----------------------------------

    void Start()
    {

    }

    void OnEnable()
    {
        DrawMap();

        GameCore.Core.mouseOverGUI = true;
    }

    void OnDisable()
    {
        ClearMap();

        GameCore.Core.mouseOverGUI = false;
    }

}