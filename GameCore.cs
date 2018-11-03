using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;  // <--- Lists


public class GameCore : MonoBehaviour
{
    string consoleOutput = 

    "\n Prototype"+
    "\n " +
    "\n version 0.0.0" +
    "\n release date: 15-09-2018"+
    "\n " +
    "\n --------- Game controls ---------" +
    "\n " +
    "\n Mouse:" +
    "\n LMB - attack" +
    "\n RMB - throw" +
    "\n " +
    "\n Keyboard:" +
    "\n W, A, D - move and jump" +
    "\n Space - pick up item / drop all items" +
    "\n " +
    "\n F - fullscreen on / off" +
    "\n C - combat mode on / off" +
    "\n O - world map on / off" +
    "\n M - spawn enemy" +
    "\n N - spawn ally" +
    "\n " +
    "\n Zoom in and out with a mouse wheel." +
    "\n " +
    "\n " +
    "\n " +
    "\n " +
    "\n "
    ;



    // =================================================== GAME CORE ========================================================

    // ------------- VARIABLES ----------------

    public static GameCore Core;

    public int     currentLand;
	public int     landSections;
	public float[] landPointX;
	public float[] landPointY;
    public bool    combatMode = true;
    public bool    travelMode = false;
    public bool    travelRight = false;

    public GameObject playerPrefab;
    public GameObject manPrefab;
    public GameObject herbiPrefab;
    public GameObject itemPrefab;
    public GameObject plantPrefab;
    public GameObject pantsPrefab;
    public GameObject hpBarPrefab;
    public GameObject platformPrefab;
    public GameObject buttonAPrefab;
    public GameObject buttonIPrefab;
    public GameObject imagePrefab;

    // runtime (ingame) prefabs required for 2d polygon collider optimisation
    GameObject sprucePrefab;
    GameObject birchPrefab;
    // --------

    public Sprite spr_man;

    public Sprite spr_spruce;
    public Sprite spr_birch;
    public Sprite spr_hemp;
    public Sprite spr_berry_bush;

    public Sprite spr_rock;
    public Sprite spr_roundRock;
    public Sprite spr_sharpRock;
    public Sprite spr_largeRock;
    public Sprite spr_flatRock;
    public Sprite spr_wood;
    public Sprite spr_meat;
    public Sprite spr_berry;
    public Sprite spr_flint;
    public Sprite spr_handaxe;
    public Sprite spr_stoneSpear;
    
    public Canvas myCanvas;
    public Mesh mesh;
    
    public Text consoleText;
    public new Text guiText;

    public GameObject player;
    public GameObject worldMap;

    GameObject clone;
    GameObject clone2;

    Land land;
    Node node;

    public List<Land> lands          = new List<Land>();
    public List<Node> nodes          = new List<Node>();
    public List<Team> teams          = new List<Team>();

    public List<GameObject> critters = new List<GameObject>();
    public List<GameObject> items    = new List<GameObject>();
    public List<GameObject> plants   = new List<GameObject>();
    public List<GameObject> buttonsA = new List<GameObject>();
    public List<GameObject> buttonsI = new List<GameObject>();

    public const int   LAND_SECTIONS = 150;
    public const float GRAVITY = 5f;
    public const float JUMP_FORCE = 12;
    public const float MOVE_FORCE = 30;

    public RaycastHit2D[] rhit2D;

    Vector3 v1 = new Vector3(75f,0,0);    // player spawn position
    Vector3 v2 = new Vector3(0,0.4f,0);   // pants relative position
    Vector3 v3 = new Vector3(80f,-20f,0); // RMB button (type A) relative position from mouse
    Vector3 v4 = new Vector3(0f,-0.5f,0); // RMB buttons (type A) position increment
    Vector3 v5 = new Vector3(80f,80f,0f); // inventory item button (type I) screen position
    Vector3 v6 = new Vector3(120f,0f,0f); // inventory items button (type I) position increment

    public Vector3 mousePos;
    public GameObject RMBclickedObj;

    public List<Vector3> verts = null;
    public List<int> tris = null;
    public List<Vector2> uvs = null;

    int i;

    // ------------ CLASSES ------------------


    public class Land
    {
        public int index;

        public int     landSections;
        public float[] landPointX;
	    public float[] landPointY;

        public Node nodeL;
        public Node nodeR;

        public List<GameObject> critters = new List<GameObject>();
        public List<GameObject> items    = new List<GameObject>();
        public List<GameObject> plants   = new List<GameObject>();

        public void GenerateLand()
        {
		    landSections = LAND_SECTIONS;

		    landPointX = new float[landSections];
		    landPointY = new float[landSections];

            // generate ground

		    int i;

		    for (i=0; i<landSections; i++)
		    {
                if (i == 0)
                landPointX[i] = 0;
                else
                landPointX[i] = landPointX[i-1] + Random.Range(1f, 2f);

                if (i == 0)
                landPointY[i] = Random.value * 5;
                else
                landPointY[i] = landPointY[i-1] + Random.Range(-1f, 1f);
		    }

            // generate plants

            int r;
            
            r = Random.Range(5,10);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.spruce, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));
            
            r = Random.Range(5,10);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.birch, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(5,10);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.hemp, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(10,20);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.berryBush, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));
            
            // generate items

            r = Random.Range(0,10);
            for (i=0; i<r; i++)
            SpawnItem(EItem.flint, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(0,10);
            for (i=0; i<r; i++)
            SpawnItem(EItem.rock, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(0,10);
            for (i=0; i<r; i++)
            SpawnItem(EItem.sharpRock, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(0,10);
            for (i=0; i<r; i++)
            SpawnItem(EItem.roundRock, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            //

        }

        // spawn plant method

        public GameObject SpawnPlant(EPlant _plant, Vector3 _position)
        {
            switch (_plant)
            {
                case EPlant.spruce:
                {
                    Core.clone = Instantiate (Core.sprucePrefab, _position, Core.transform.rotation) as GameObject;
                    break;
                }

                case EPlant.birch:
                {
                    Core.clone = Instantiate (Core.birchPrefab, _position, Core.transform.rotation) as GameObject;
                    break;
                }

                case EPlant.berryBush:
                {
                    Core.clone = Instantiate (Core.plantPrefab, _position, Core.transform.rotation) as GameObject;
                    break;
                }

                case EPlant.hemp:
                {
                    Core.clone = Instantiate (Core.plantPrefab, _position, Core.transform.rotation) as GameObject;
                    break;
                }    
            }

            if (index == Core.currentLand)
            {
                Core.plants.Add(Core.clone);
            }
            else
            {
                plants.Add(Core.clone);
                Core.clone.SetActive(false);
            }

            Core.clone.GetComponent<PlantCore>().plant = _plant;
            Core.clone.transform.position = new Vector3(Core.clone.transform.position.x, Core.clone.transform.position.y, 0f);

            return Core.clone;
        }

        // spawn item

        public GameObject SpawnItem(EItem _item, Vector3 _position)
        {
            Core.clone = Instantiate (Core.itemPrefab, _position, Core.transform.rotation) as GameObject;

            if (index == Core.currentLand)
            {
                Core.items.Add(Core.clone);
            }
            else
            {
                items.Add(Core.clone);
                Core.clone.SetActive(false);
            }

            Core.clone.GetComponent<ItemCore>().item = _item;
            Core.clone.transform.position = new Vector3(Core.clone.transform.position.x, Core.clone.transform.position.y, 0f);

            return Core.clone;
        }
    }

    // ---------------------------------------

    public class Node
    {
        public int a, b;        // world coordinates
        public bool visited;    // if node was visited by player

        public Node()
        {
            a = 0;
            b = 0;
            visited = false;
        }
    }

    // ---------------------------------------

    public class Team
    {
        public int index;

        public List<GameObject> members = new List<GameObject>();

    }

    // ---------------------------------------

    //  ---------------- METHODS ------------------

    /// InitializeIngamePrefabs()
    /// SetupFirstLand()
    /// LoadLand()
    /// DrawLand()
    /// RedrawLand()
    /// ExploreNode(2)

    /// SpawnPlayer()
    /// SpawnMan(1)
    /// SpawnHerbi()
    /// SpawnItem(1)
    /// SpawnPlant(2)
    
    /// HighlightObjectUnderMouse()
    /// RMBManager()
    /// AddButtonA(3)
    /// InventoryManager()
    /// AddButtonI(1)

    //-----------------------------------------------------

    public void SetupFirstLand()
    {
		landSections = LAND_SECTIONS;

		landPointX = new float[landSections];
		landPointY = new float[landSections];

        // initialize array

		for (i=0; i<landSections; i++)
		{
            landPointX[i] = 0;
            landPointY[i] = 0;
		}

        // ------- generate first land create and two nodes ---------

        land = new Land();
        lands.Add(land);
        lands[0].GenerateLand();

        //

        node = new Node();
        nodes.Add(node);
        lands[0].nodeR = node;

        node.a = 0;
        node.b = 0;

        //

        node = new Node();
        nodes.Add(node);
        lands[0].nodeL = node;

        node.a = 1;
        node.b = 0;



    }

    //-----------------------------------------------------

    public void InitializeIngamePrefabs()
    {
        // ingame prefabs are required for 2d polygon collider optimisation
        // they are actual game objects, but deactivated

        // spruce
        sprucePrefab = Instantiate (plantPrefab, transform.position, transform.rotation) as GameObject;
        sprucePrefab.GetComponent<PlantCore>().plant = EPlant.spruce;
        sprucePrefab.GetComponent<PlantCore>().PlantInitialize();
        sprucePrefab.GetComponent<PlantCore>().BodyInitialize();
        sprucePrefab.GetComponent<PlantCore>().CreatePolygonCollider();
        sprucePrefab.GetComponent<PlantCore>().OptimizePolygonCollider(2.0f);
        sprucePrefab.SetActive(false); 

        // birch
        birchPrefab = Instantiate (plantPrefab, transform.position, transform.rotation) as GameObject;
        Core.birchPrefab.GetComponent<PlantCore>().plant = EPlant.birch;
        Core.birchPrefab.GetComponent<PlantCore>().PlantInitialize();
        Core.birchPrefab.GetComponent<PlantCore>().BodyInitialize();
        Core.birchPrefab.GetComponent<PlantCore>().CreatePolygonCollider();
        Core.birchPrefab.GetComponent<PlantCore>().OptimizePolygonCollider(0.21f);
        Core.birchPrefab.SetActive(false); 
    }

    //-----------------------------------------------------

    public void LoadLand(int index)
    {
        // save objects and deacivate them

        lands[currentLand].plants.Clear();
        
        for (i=0; i<plants.Count; i++)
        {
            lands[currentLand].plants.Add(plants[i]);
            plants[i].SetActive(false);
        }
        
        plants.Clear();

        lands[currentLand].items.Clear();


        for (i=0; i<items.Count; i++)
        {
            // only non-carried items are saved and deactivated
            if (items[i].GetComponent<ItemCore>().isCarried == false)
            {
                lands[currentLand].items.Add(items[i]);
                items[i].SetActive(false);
            }
        }

        
        items.Clear();

        
        lands[currentLand].critters.Clear();
        
        for (i=0; i<critters.Count; i++)
        {
            lands[currentLand].critters.Add(critters[i]);
            critters[i].SetActive(false);
        }
        
        critters.Clear();
        
        // --------------------------
        
        // load a new land

        currentLand = index;

        if (player)
        player.SetActive(true);

        landSections = lands[index].landSections;

		for (i=0; i<landSections; i++)
		{
            landPointX[i] = lands[index].landPointX[i];
            landPointY[i] = lands[index].landPointY[i];
		}

        // load objects and activate them

        for (i=0; i<lands[currentLand].plants.Count; i++)
        {
            plants.Add(lands[currentLand].plants[i]);
            lands[currentLand].plants[i].SetActive(true);
        }

        for (i=0; i<lands[currentLand].items.Count; i++)
        {
            items.Add(lands[currentLand].items[i]);
            lands[currentLand].items[i].SetActive(true);
        }     
        
        for (i=0; i<lands[currentLand].critters.Count; i++)
        {
            critters.Add(lands[currentLand].critters[i]);
            lands[currentLand].critters[i].SetActive(true);
        }  

        // not much optimized :/
        DrawLand();
        RedrawLand();
        //
    }

    //-----------------------------------------------------

    void DrawLand()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        
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

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        //mesh.SetUVs(0,uvs);

        mesh.RecalculateBounds();
    }

    //-----------------------------------------------------

    void RedrawLand()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        int j = 0;
        
        for (i=0; i<landSections-1; i++)
        {
            if (landPointY[i+1] > landPointY[i])
            {
                verts[j] = (new Vector3(landPointX[i], landPointY[i], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i+1], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i], 0));
                j++;

                verts[j] = (new Vector3(landPointX[i], landPointY[i], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i]-100f, 0));
                j++;

                verts[j] = (new Vector3(landPointX[i], landPointY[i], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i]-100f, 0));
                j++;
                verts[j] = (new Vector3(landPointX[i], landPointY[i]-100f, 0));
                j++;
            }
            else
            {
                verts[j] = (new Vector3(landPointX[i], landPointY[i], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i+1], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i], landPointY[i+1], 0));
                j++;

                verts[j] = (new Vector3(landPointX[i], landPointY[i+1], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i+1], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i]-100f, 0));
                j++;

                verts[j] = (new Vector3(landPointX[i], landPointY[i+1], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i]-100f, 0));
                j++;
                verts[j] = (new Vector3(landPointX[i], landPointY[i]-100f, 0));
                j++;
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
    }

    //-----------------------------------------------------

    void ExploreNodeL(int _a, int _b)
    {
        bool thisNodeExists = false;

        for (i=0; i<nodes.Count; i++)
        {
            if ((nodes[i].a == lands[currentLand].nodeL.a + _a)
            && (nodes[i].b == lands[currentLand].nodeL.b + _b))
            {
                thisNodeExists = true;
                break; // if node exists, stop searching
            } 
            // if not, keep searching
        }

        if (thisNodeExists == false)  // if node not found
        {
            // generate new node

            node = new Node();
            nodes.Add(node);
            node.a = lands[currentLand].nodeL.a + _a;
            node.b = lands[currentLand].nodeL.b + _b;

            // generate new land towards new node

            land = new Land();
            lands.Add(land);
            land.index = lands.Count-1;
            land.GenerateLand();
            

            if ((_a == 1) || (_b == -1))
            {
                land.nodeL = node;
                land.nodeR = lands[currentLand].nodeL;
            }
            else
            {
                land.nodeL = lands[currentLand].nodeL;
                land.nodeR = node;
            }
        }
    }

    //-----------------------------------------------------

    void ExploreNodeR(int _a, int _b)
    {
        bool thisNodeExists = false;

        for (i=0; i<nodes.Count; i++)
        {
            if ((nodes[i].a == lands[currentLand].nodeR.a + _a)
            && (nodes[i].b == lands[currentLand].nodeR.b + _b))
            {                
                thisNodeExists = true;
                break; // if node exists, stop searching
            }
            // if not, keep searching
        }

        if (thisNodeExists == false) // if node not found
        {
            // generate new node

            node = new Node();
            nodes.Add(node);
            node.a = lands[currentLand].nodeR.a + _a;
            node.b = lands[currentLand].nodeR.b + _b;

            // generate new land towards new node

            land = new Land();
            lands.Add(land);
            land.index = lands.Count-1;
            land.GenerateLand();

            if ((_a == 1) || (_b == -1))
            {
                land.nodeL = node;
                land.nodeR = lands[currentLand].nodeR;
            }
            else
            {
                land.nodeL = lands[currentLand].nodeR;
                land.nodeR = node;
            }
        }
    }

    //-----------------------------------------------------

    void HighlightObjectUnderMouse()
    {
        // ---- HIGHLIGHTING OBJECTS UNDER MOUSE ----

        // 1. make previous objects white

        if (rhit2D.Length > 0) // BUG HERE !!  NullReferenceException: Object reference not set to an instance of an object
        {
            for (i=0; i < rhit2D.Length; i++)
            {
                // BUG HERE !
                // NullReferenceException: Object reference not set to an instance of an object.
                if (rhit2D[i])
                rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
            }
        }
        // 2. find new objects under mouse

        rhit2D = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), new Vector2(0f,0f));
        
        // 3. make new objects red

        if (rhit2D.Length > 0)
        {
            for (i=0; i < rhit2D.Length; i++)
            {
                rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f,0.7f,0.7f,1f);
            }
        }
    }

    //-----------------------------------------------------

    void RMBManager()
    {
        i = 0;

        {
            AddButtonA(i,"move here",EAction.move);
            i++;
        }

        if (rhit2D.Length > 0)
        {
            if (rhit2D[0].transform.gameObject.GetComponent<InteractiveObjectCore>().kind != EKind.none)
            {
                RMBclickedObj = rhit2D[0].transform.gameObject;

                if (RMBclickedObj.GetComponent<InteractiveObjectCore>().kind == EKind.item)
                {
                    AddButtonA(i,"pickup",EAction.pickup);
                    i++;

                    if (RMBclickedObj.GetComponent<ItemCore>().item == EItem.flint)
                    {
                        AddButtonA(i,"craft handaxe",EAction.craftHandaxe);
                        i++;

                    }

                }

                if (RMBclickedObj.GetComponent<InteractiveObjectCore>().kind == EKind.plant)
                {
                    if (RMBclickedObj.GetComponent<PlantCore>().plant == EPlant.spruce)
                    {
                        AddButtonA(i,"chop",EAction.chop);
                        i++;

                        AddButtonA(i,"gather firewood",EAction.gather_wood);
                        i++;

                        AddButtonA(i,"gather plant material",EAction.gather_plant_material);
                        i++;
                    }
                    else
                    {
                        AddButtonA(i,"cut down",EAction.cut_down);
                        i++;
                    }

                }
            }
        }
        else
        {
            if (player.GetComponent<CritterCore>().carriedBodies.Count > 0)
            {
                AddButtonA(i,"drop all here",EAction.drop_all);
                i++;
            }
        }
    }

    //-----------------------------------------------------

    void AddButtonA(int index, string label, EAction action)
    {
        int j;    

        clone = Instantiate(buttonAPrefab, transform.position, transform.rotation) as GameObject;
        buttonsA.Add(clone);
        clone.transform.SetParent(myCanvas.transform,false);
        clone.GetComponent<ButtonACore>().pos = Camera.main.ScreenToWorldPoint(Input.mousePosition + v3);
        clone.GetComponent<ButtonACore>().obj = RMBclickedObj;

        if (index > 0)
        {
            for (j=0; j<index; j++)
            clone.GetComponent<ButtonACore>().pos += v4;
        }

        clone.GetComponentInChildren<Text>().text = label;
        clone.GetComponent<ButtonACore>().action = action;
    }

    //-----------------------------------------------------

    public void InventoryManager()
    {
        // clear existing buttons (type I)

        for (i=0; i<=buttonsI.Count-1; i++)
        {
            Destroy(buttonsI[i]);
        }

        buttonsI.Clear();

        // create new buttons (type I)

        if (player.GetComponent<ManCore>().carriedBodies.Count > 0)
        {
            for (i=0; i < player.GetComponent<ManCore>().carriedBodies.Count; i++)
            {
                AddButtonI(i,0);
            }
        }

        if (player.GetComponent<ManCore>().tool != null)
        {
            AddButtonI(i,1);
        }
    }

    //-----------------------------------------------------

    public void AddButtonI(int index, int type)
    {
        clone = Instantiate(buttonIPrefab, transform.position, transform.rotation) as GameObject;
        buttonsI.Add(clone);
        clone.transform.SetParent(myCanvas.transform,false);
        clone.GetComponent<ButtonICore>().type = type;

        clone2 = Instantiate(imagePrefab, transform.position, transform.rotation) as GameObject;
        clone2.transform.SetParent(clone.transform,false);

        if (type == 0) // item in inventory
        {
            clone.transform.position = v5 + v6*i;
            clone.GetComponent<ButtonICore>().index = index;

            clone2.GetComponent<Image>().sprite = player.GetComponent<CritterCore>().carriedBodies[index].GetComponent<SpriteRenderer>().sprite;
        }
        else
        if (type == 1) // tool slot
        {
            clone.transform.position = v5 + new Vector3(0f,100f,0f);

            clone2.GetComponent<Image>().sprite = player.GetComponent<ManCore>().tool.GetComponent<SpriteRenderer>().sprite;
        }
    }

    //-----------------------------------------------------

    public void SpawnPlayer()
    {
        if (player == null)
        {
            player = Instantiate (playerPrefab, transform.position + v1, transform.rotation) as GameObject;
            critters.Add(player);

            // pants

            clone = Instantiate (pantsPrefab,transform.position,transform.rotation) as GameObject;
            clone.transform.parent = player.transform;
            clone.transform.position = clone.transform.parent.position + v2;
            clone.GetComponent<SpriteRenderer>().color = Color.blue;

            // hp bar

            clone = Instantiate(hpBarPrefab, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<HpBarCore>().parent = player;
        }
    }

    //-----------------------------------------------------

    public void SpawnMan(int _team)
    {
        clone = Instantiate (manPrefab, mousePos, transform.rotation) as GameObject;
        clone.name = "Man";
        clone.transform.position = new Vector3(clone.transform.position.x,clone.transform.position.y,1f);
        clone.GetComponent<ManCore>().team = _team;
        teams[_team].members.Add(clone);
        critters.Add(clone);

        if (_team == 1)
        {
            clone.GetComponent<ManCore>().command = EAction.follow;
            clone.GetComponent<ManCore>().target = player;
        }
        else
        if (_team == 2)
        {
            clone.GetComponent<ManCore>().command = EAction.idle;
        }

        // pants
        
        clone2 = Instantiate (pantsPrefab,transform.position, transform.rotation) as GameObject;
        clone2.transform.parent = clone.transform;
        clone2.transform.position = clone.transform.position + v2;

        if (_team == 1)
        clone2.GetComponent<SpriteRenderer>().color = Color.blue;
        else if (_team == 2)
        clone2.GetComponent<SpriteRenderer>().color = Color.red;

        // hp bar

        clone2 = Instantiate(hpBarPrefab, transform.position, transform.rotation) as GameObject;
        clone2.GetComponent<HpBarCore>().parent = clone;
    }

    //-----------------------------------------------------

    public void SpawnHerbi()
    {
        clone = Instantiate (herbiPrefab, mousePos, transform.rotation) as GameObject;
        critters.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);
        teams[1].members.Add(clone);
        clone.GetComponent<HerbiCore>().command = EAction.idle;

        // hp bar

        clone2 = Instantiate(hpBarPrefab, transform.position, transform.rotation) as GameObject;
        clone2.GetComponent<HpBarCore>().parent = clone;
    }

    //-----------------------------------------------------
    
    public GameObject SpawnItem(EItem _item)
    {
        clone = Instantiate (itemPrefab, mousePos, transform.rotation) as GameObject;
        items.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

        clone.GetComponent<ItemCore>().item = _item;

        return clone;
    }

    //-----------------------------------------------------

    public void SpawnPlatform()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        clone = Instantiate (platformPrefab, mousePos, transform.rotation) as GameObject;
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);
       
    }

    //-----------------------------------------------------


    // ============================================================ MAIN LOOP ==================================================================

    /// ----- AWAKE -----

	void Awake()
	{
        Core = gameObject.GetComponent<GameCore>();

        InitializeIngamePrefabs();
        SetupFirstLand();

        // raycast initialization
        rhit2D = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), new Vector2(0f,0f));
        //

        teams.Add(new Team()); // Team 0 -> Pacifistic Team: Permanently peaceful animals and humans
        teams[0].index = 0;

        teams.Add(new Team()); // Team 1 -> Player Team
        teams[1].index = 1;

        teams.Add(new Team()); // Team 2 -> Bad Guys Team
        teams[2].index = 2;

        LoadLand(0);

        SpawnPlayer();
        teams[1].members.Add(player);

        consoleText.text = consoleOutput;
	}

    /// ----- START -----

	void Start() 
	{
        //Application.targetFrameRate = -1; // for performance check (remember to turn v-sync off)
	}

    /// ----- UPDATE -----
     
	void Update()
	{
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // input
        /*
        if (Input.GetKeyDown(KeyCode.I))
		lands[currentLand].SpawnItem(EItem.sharpRock, mousePos);

        if (Input.GetKeyDown(KeyCode.S))
		SpawnItem(EItem.stoneSpear);

        if (Input.GetKeyDown(KeyCode.F))
		SpawnItem(EItem.handaxe);
        */
        if (Input.GetKeyDown(KeyCode.M))
        SpawnMan(2);

        if (Input.GetKeyDown(KeyCode.N))
        SpawnMan(1);

        if (Input.GetKeyDown(KeyCode.O))
        {
            worldMap.SetActive(!worldMap.activeSelf);
        }
        /*
        if (Input.GetKeyDown(KeyCode.H))
        SpawnHerbi();

        if (Input.GetKeyDown(KeyCode.T))
        lands[currentLand].SpawnPlant(EPlant.spruce, mousePos);

        if (Input.GetKeyDown(KeyCode.P))
        SpawnPlatform();

        
        if (Input.GetKeyDown(KeyCode.C))
        {
            combatMode = !combatMode;

            // clear existing buttons of type A

            for (i=0; i<=buttonsA.Count-1; i++)
            {
                Destroy(buttonsA[i]);
            }

            buttonsA.Clear();
            RMBclickedObj = null;

            //
        }
        */

        // switch to fullscreen (works only in standalone)
		if (Input.GetKeyDown(KeyCode.F))
		Screen.fullScreen = !Screen.fullScreen;
        //


        HighlightObjectUnderMouse();

        // ----- LEFT MOUSE BUTTON ------

        if(Input.GetMouseButtonUp(0))
        {
            if (combatMode == false)
            {
                // clear buttons (type A)    

                for (i=0; i<=buttonsA.Count-1; i++)
                {
                    Destroy(buttonsA[i]);
                }

                buttonsA.Clear();
                RMBclickedObj = null;
            }
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (combatMode == false) // fixes player slightly moving  when hitting bug
            {
                // move player
                
                player.GetComponent<CritterCore>().command = EAction.move;
                player.GetComponent<CritterCore>().target = null;
                
                Vector3 v1;
                v1 =  Camera.main.ScreenToWorldPoint(Input.mousePosition);

                player.GetComponent<CritterCore>().targetX = v1.x;
            }
        }

        // ----- RIGHT MOUSE BUTTON ------

        if(Input.GetMouseButtonDown(1))
        {
            if (combatMode == false)
            {
                // clear buttons (type A)  

                for (i=0; i<=buttonsA.Count-1; i++)
                {
                    Destroy(buttonsA[i]);
                }

                buttonsA.Clear();
                RMBclickedObj = null;

                // check for new objects under mouse

                RMBManager();
            }
        }

        // ----------------------------

        // combat mode text

        if (guiText)
        {
            if (combatMode == false)
            guiText.text = " FPS: "+(1.0f / Time.deltaTime).ToString();//+"\n\n Combat mode OFF";
            else
            guiText.text = " FPS: "+(1.0f / Time.deltaTime).ToString();//+"\n\n Combat mode ON";
        }
    }

    /// ----- LATE UPDATE -----

    // ---------------------------------------------- WORLD TRAVEL ------------------------------------------------
    /*
    void LateUpdate()
    {

        // ---------- left edge of the land --------------

        if (player.GetComponent<BodyCore>().landSection < 30) 
        {
            travelRight = false;

            if (travelMode == false) // enter travel mode
            {
                travelMode = true;
                worldMap.SetActive(true);

                if (lands[currentLand].nodeL.visited == false) // if this node is not yet visited
                {
                    lands[currentLand].nodeL.visited = true;

                    // check existing nodes and create new ones

                    ExploreNodeL(1,0);
                    ExploreNodeL(0,1);
                    ExploreNodeL(-1,0);
                    ExploreNodeL(0,-1);
                }

                worldMap.GetComponent<WorldMapCore>().ClearMap();
                worldMap.GetComponent<WorldMapCore>().DrawMap();
            }
        }
        // ----------- right edge of the land -----------

        else
        if (player.GetComponent<BodyCore>().landSection > (landSections-1)-30) 
        {
            travelRight = true;

            if (travelMode == false) // enter travel mode
            {
                travelMode = true;
                worldMap.SetActive(true);

                if (lands[currentLand].nodeR.visited == false) // if this node is not yet visited
                {
                    lands[currentLand].nodeR.visited = true;

                    // check existing nodes and create new

                    ExploreNodeR(1,0);
                    ExploreNodeR(0,1);
                    ExploreNodeR(-1,0);
                    ExploreNodeR(0,-1);
                }

                worldMap.GetComponent<WorldMapCore>().ClearMap();
                worldMap.GetComponent<WorldMapCore>().DrawMap();
            }
        }

        // -----------------------------------------------

        else
        {
            // leave travel mode

            if (travelMode == true) 
            {
                worldMap.SetActive(false);
            }

            travelMode = false;
        }
        

    // ------------------------------------------------------------------------------------------------




	}

    // ------------------------------------------------------------------------------------------------------------
    */
}
