﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;  // <--- enables lists
using UnityEngine.EventSystems;

public enum EAction {none, idle, moveRight, moveLeft, move, follow, attack, pickUp, /*dropAll ,*/ eat, chop, cutDown,
                     craftHandAxe, obtainMeat, convert, craftStoneSpear, runAway, setOnFire, processTree, putItemInFireplace,
                     deleteProject, openBuildPanel, buildShelter, setItemInFire, setItemHeated, heatItem, heating, giveItem,
                     setGatheringPoint, deleteGatheringPoint, craftCordage, craftBarkTorch, continueProject, equip, unequip,
                     drop};

public enum EWorldEvent {none, carniEnter, unitRandomMove, spawnCarniPack};

public class GameCore : MonoBehaviour
{
    string consoleOutput = 
    
    "\n Prototype"+ 
    "\n Version: 0.0.9 WIP"+
    "\n"+
    "\n Tasks for today:"+
    "\n - bug: space bar item picking"+
    "\n - disable zoom and camera movement in Esc Menu"+
    "\n"+
    "\n To do for Tech Demo 1:"+
    "\n - cutting grass with bare hands"+
    "\n - bug: when having buttonO of a project and this project is then finished, the buttonO remains and generates an error"+
    "\n - big text in the middle od the screen f.e. 'choose item from inventory', 'objective completed'"+
    "\n - remove travel, remove all critters, remove world map and build panel, remove combat mode, decrease zoom range, remove most input"+

    "\n To do later:"+
    "\n - bug: herbi lagging when unpausing game"+
    "\n - bug: stacking hudText objects, world map gets bugged probably by invisible hudText objects"+
    "\n - bug: wrong sky position in lands other than the first"+
    "\n - bug: sometimes cursor label doesn't disappear after adding bark torch to fire"+
    "\n - mouseOverGUI should be reworked into raycasting method"+
    "\n - hiding buttons type I when world map or build panel is on"+
    "\n - critters running away to adjacent lands"+
    "\n - different tool damage and speed"+
    "\n - thrust spears instead of swinging"+
    "\n - critters slowing down when moving uphill"+
    "\n - critters slowing down when wounded"+
    "\n - two hand slots "+
    "\n - hp and progress bars should be on the canvas (not sure)"+
    "\n - ai throwing"+
    "\n - world map scrolling rework"+
    "\n - more coroutines: all timers, checking world events"+
    "\n - critter flyout texts (displayed f.e. when gaining points or state changes)"+

    "\n"+
    "\n Done:" +
    "\n - UI: darkened buttons if requirements not met"+
    "\n - improved LMB movement"+
    "\n - progress bars visible only if progress > 0f"+
    "\n - loading projects between lands"+
    "\n - fixed: loading converted tribesmen between lands"+
    "\n - UI: cursor label text over buttons type I"+
    "\n - fixed: project hit colorizing"+
    "\n - fixed: putting items in the fire"+
    "\n - LMB actions: pick up / equip - when combat mode is off"+
    "\n - campfires and items burning out"+
    "\n - extended sky"+
    "\n - hide project sprite for heating"+
    "\n - fixed: critters are now unable to move beyond edges of the land"+
    "\n - new action: continue project"+
    "\n - interacting with carried items: drop, equip, unequip; HUD is now optional"+
    "\n - prevent critters from moving beyond edges of the land"+
    "\n - ability to burn grass and bushes in campfire"+
    "\n - darken rest of buttons type A: process tree, obtain meat, craft hand axe"+
    "\n - setting firewood on fire with a bark torch"+
    "\n - fixed: carried plants UI interaction"+
    "\n - rotated carried tool items"+
    "\n - fixed: carried objects sorting groups"+
    "\n - fixed: fit all 2D colliders to sprites"+
    "\n - improved man sprite"+
    "\n - UI visual overhaul"+
    "\n - menu shown when ESC pressed"+
    "\n - quitting and restarting game"+
    "\n - pausing / unpausing the game"+
    "\n - round rock changed renamed to hammerstone"+
    "\n - sticks obtained from processed trees"+
    "\n - help screen"+
    "\n - crafting without hitting -> project self progressing, but only when action is to craft"+
    "\n - ";

    /*
     
    "\n Version: 0.0.8"+ 
    "\n release date: 26-12-2019"+


    "\n"+
    "\n Done:" +
    "\n - new plant: grass"+
    "\n - new item: bark torch"+
    "\n - reworked fire visuals (lights, particles)"+
    "\n - reworked sky and day/night cycle visuals"+
    "\n - big optimizations" +
    "\n - added hp and progress bars"+
    "\n - reworked object info under cursor (minimum info displayed)"+
    "\n - flickering fire lights"+
    "\n - campfire's fire size depends on amount of fuel added"+
    "\n - message texts (talking)"+
    "\n - equipped items displayed at the top of the screen, unequipped at the bottom"+
    "\n - cooking meat fixed"+
    "\n - converting wildmen by giving them food"+
    "\n - item: stick"+
    "\n - crafting torches from cordage, birch bark, stick"+
    "\n - torches burning out into sticks"+
    "\n - hide edges of the land"+
    "\n - cutting and processing trees related to tree type and age"+
    "\n - reworked mouse object highlighting"+
    "\n - pulsating highlight when mouse over button type O and A; non-pulsating highlight when mouse over object"+

    // ==============================================

    */
    

    // =================================================== GAME ========================================================

    // ------------- VARIABLES ----------------

    public static GameCore Core;

    public GameObject playerPrefab;
    public GameObject manPrefab;
    public GameObject herbiPrefab;
    public GameObject carniPrefab;
    public GameObject itemPrefab;
    public GameObject plantPrefab;
    public GameObject structurePrefab;
    public GameObject pantsPrefab;
    public GameObject hpBarPrefab;
    public GameObject platformPrefab;
    public GameObject buttonAPrefab;
    public GameObject buttonIPrefab;
    public GameObject buttonOPrefab;
    public GameObject imagePrefab;
    public GameObject projectPrefab;
    public GameObject firePrefab;
    public GameObject fireplacePrefab;
    public GameObject sunlightPrefab;
    public GameObject shelterPrefab;
    public GameObject progressBarPrefab;
    public GameObject progressBarFramePrefab;
    public GameObject messageTextPrefab;
    public GameObject treePrefab;

    public GameObject hudTextPrefab; // <-- !


    // runtime (ingame) prefabs required for 2d polygon collider optimisation
    GameObject sprucePrefab;
    GameObject birchPrefab;
    // --------

    public Sprite spr_unit;

    public Sprite spr_spruce;
    public Sprite spr_birch;
    public Sprite spr_hemp;
    public Sprite spr_berry_bush;
    public Sprite spr_grass;

    public Sprite spr_rock;
    public Sprite spr_hammerstone;
    public Sprite spr_sharpRock;
    public Sprite spr_largeRock;
    public Sprite spr_flatRock;
    public Sprite spr_wood;
    public Sprite spr_meat;
    public Sprite spr_berry;
    public Sprite spr_flint;
    public Sprite spr_handAxe;
    public Sprite spr_stoneSpear;
    public Sprite spr_fibers;
    public Sprite spr_smallLog;
    public Sprite spr_campfire;
    public Sprite spr_plantMaterial;
    public Sprite spr_firewood;
    public Sprite spr_bark;
    public Sprite spr_cordage;
    public Sprite spr_cookedMeat;
    public Sprite spr_chick;
    public Sprite spr_deer;
    public Sprite spr_barkTorch;
    public Sprite spr_stick;

    public Sprite spr_shelter;

 // ----------------------------------------------

    public const int   LAND_SECTIONS = 500;
    public const float GRAVITY = 4f;
    public const float JUMP_FORCE = 12f;
    public const float MOVE_FORCE = 35f;

    public float   timeStamp;
    public int     timeDay;
    public int     timeHour;
    public float   timeMinute = 0f;
    public float   timePrevious = 0f; // Time.time value from previous frame

    public int     currentLand;
	public int     landSections;
	public float[] landPointX;
	public float[] landPointY;

    public GameObject ground;
    public GameObject background1;
    public GameObject background2;
    public GameObject background3;

    public GameObject player;
    public GameObject cursorLabel;
    public GameObject worldMap;
    public GameObject sunlight;
    public GameObject buildPanel;
    public GameObject ghostObject; // used for placing structures
    public GameObject sky;
    public GameObject escMenu;
    public GameObject helpPanel;

    public Vector3 mousePos;
    public GameObject RMBclickedObj;
    public GameObject chosenObject; // object chosen using "choose from inventory" mode

    public bool combatMode;
    public bool mouseOverGUI = false;
    public bool chooseFromInventoryMode = false; // "choose from inventory" mode (e.g. adding fuel to campfire)
    public bool hideHUD = false;
    public bool gamePaused = false;

    public bool travelMode = false; // traveling the world map
    public bool travelRight = false;

    public float gatheringPointX;
    public GameObject gatheringPoint;

    public Canvas myCanvas;
    public Mesh mesh;
    
    public Text consoleText;
    public new Text guiText;
    public Text combatModeText;

    Land land;
    Node node;

    public List<Land> lands = new List<Land>();
    public List<Node> nodes = new List<Node>();
    public List<Team> teams = new List<Team>();
    public List<Unit> units = new List<Unit>();
    public List<WorldEvent> events = new List<WorldEvent>();

    public List<GameObject> critters   = new List<GameObject>();
    public List<GameObject> items      = new List<GameObject>();
    public List<GameObject> plants     = new List<GameObject>();
    public List<GameObject> structures = new List<GameObject>();
    public List<GameObject> projects   = new List<GameObject>();

    public List<GameObject> buttonsA = new List<GameObject>();
    public List<GameObject> buttonsI = new List<GameObject>();
    public List<GameObject> buttonsO = new List<GameObject>();
    public List<GameObject> buttonsE = new List<GameObject>();

    public RaycastHit2D[] rhit2D;

    // note: setting variables as below, doesn't work 

    public Vector3 v1 = new Vector3(110f,0,0);    // player spawn position
    public Vector3 v2 = new Vector3(0,0.4f,0);    // pants relative position
    public Vector3 v3 = new Vector3(150f,-10f,0); // RMB button (type A) relative position from mouse
    public Vector3 v4 = new Vector3(0f,-0.5f,0);  // RMB buttons (type A) position increment
    public Vector3 v5 = new Vector3(80f,80f,0f);  // inventory item button (type I) screen position
    public Vector3 v6 = new Vector3(120f,0f,0f);  // inventory items button (type I) position increment
    public Vector3 v7 = new Vector3(120f,0f,0f);  // RMB button (type O) relative position from mouse

    //

    public List<Vector3> verts = null;
    public List<int> tris = null;
    public List<Vector2> uvs = null;

    GameObject clone;
    GameObject clone2;

    int i;


    //  -------------------------------------------  METHODS ---------------------------------------------

    /// SetupFirstLand()
    /// InitializeIngamePrefabs()
    
    /// LoadLand()
    /// ExploreNodeL(2)
    /// ExploreNodeR(2)
    /// CreateWorldEvent(1)
    /// ExecuteWorldEvent(1)

    /// SpawnPlayer()
    /// SpawnMan(1)
    /// SpawnHerbi()
    /// SpawnItem(1)
    /// SpawnPlant(2)

    /// LeftMouseButton()
    /// RightMouseButton()
    /// HighlightObjectUnderMouse()
    /// CreateButtonAList()
    /// RMBManager()
    /// AddButtonA(3)
    /// InventoryManager()
    /// AddButtonI(1)
    /// AddButtonO(1)
    /// PauseGame()
    /// UnpauseGame()

    // ------------ CLASSES ------------------

    public class Land
    {
        public int index;

        public int     landSections;
        public float[] landPointX;
	    public float[] landPointY;

        public Node nodeL;
        public Node nodeR;

        public List<GameObject> critters   = new List<GameObject>();
        public List<GameObject> structures = new List<GameObject>();
        public List<GameObject> items      = new List<GameObject>();
        public List<GameObject> plants     = new List<GameObject>();
        public List<GameObject> projects   = new List<GameObject>();

        int i;

        // generate land method

        public void GenerateLand()
        {
		    landSections = LAND_SECTIONS;

		    landPointX = new float[landSections];
		    landPointY = new float[landSections];

            int i;

            // generate ground (rocky)
            /*
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
            */

            // generate ground (hills)

            float r1;
            r1 = Random.Range(-1f, 1f) *0.5f;

            int j = 0;

		    for (i=0; i<landSections; i++)
		    {
                if (i == 0)
                landPointX[i] = 0;
                else
                landPointX[i] = landPointX[i-1] + 1f;// + Random.Range(1f, 2f);

                if (i == 0)
                landPointY[i] = Random.value * 0;
                else
                {
                    landPointY[i] = landPointY[i-1] + r1 + Random.Range(-1f, 1f) *0.2f;
                    j++;

                    if (j == 10)
                    {
                        r1 += Random.Range(-1f, 1f) /3;
                    }
                    if (j == 20)
                    {
                        r1 += Random.Range(-1f, 1f) /3;
                    }
                    if (j == 30)
                    {
                        r1 = Random.Range(-1f, 1f) /3;
                        j = 0;
                    }

                }
		    }

            //

            // generate plants

            int r;
            
            r = Random.Range(5,200);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.spruce, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));
            
            r = Random.Range(5,50);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.birch, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(5,200);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.grass, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(5,10);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.hemp, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(5,100);
            for (i=0; i<r; i++)
            SpawnPlant(EPlant.berryBush, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));
            
            // generate items

            r = Random.Range(2,5);
            for (i=0; i<r; i++)
            SpawnItem(EItem.flint, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(2,5);
            for (i=0; i<r; i++)
            SpawnItem(EItem.rock, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(2,5);
            for (i=0; i<r; i++)
            SpawnItem(EItem.sharpRock, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(2,5);
            for (i=0; i<r; i++)
            SpawnItem(EItem.hammerstone, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));

            r = Random.Range(2,5);
            //r += 5;
            for (i=0; i<r; i++)
            SpawnItem(EItem.flatRock, new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));
            
            // generate critters
            
            r = Random.Range(2,5);
            for (i=0; i<r; i++)
            SpawnHerbi(new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f), EHerbi.chick);
            
            r = Random.Range(2,5);
            for (i=0; i<r; i++)
            SpawnHerbi(new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f), EHerbi.deer);


            //SpawnCarniPack(new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f));
            
            r = Random.Range(0,2); // 0-1
            for (i=0; i<r; i++)
            SpawnMan(new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f), 0);
            
            r = Random.Range(0,5); // 0-4
            if (r == 0)            // 20 %
            SpawnNewTribeGroup();

        }

        // ------------------------------------------------------------------

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

                default:
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

        // spawn item method

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

        // spawn herbi method

        public GameObject SpawnHerbi(Vector3 _position, EHerbi _herbi)
        {
            Core.clone = Instantiate (Core.herbiPrefab, _position, Core.transform.rotation) as GameObject;
            Core.clone.GetComponent<HerbiCore>().herbi = _herbi;

            if (index == Core.currentLand)
            {
                Core.critters.Add(Core.clone);
            }
            else
            {
                critters.Add(Core.clone);
                Core.clone.SetActive(false);
            }
            
            Core.clone.transform.position = new Vector3(Core.clone.transform.position.x, Core.clone.transform.position.y, 0f);
            
            Core.teams[0].members.Add(Core.clone);
            Core.clone.GetComponent<HerbiCore>().action = EAction.idle;
            
            return Core.clone;
        }

        // spawn carni pack method

        public void SpawnCarniPack(Vector3 _position)
        {
            // add new team

            Core.teams.Add(new Team());
            Team _team = Core.teams[Core.teams.Count-1];
            _team.index = Core.teams.Count-1;
            _team.name = "Wolf Team #"+_team.index;
            _team.color = Color.magenta;

            // add new unit

            _team.units.Add(new Unit());
            Unit _unit = _team.units[_team.units.Count-1];
            Core.units.Add(_unit);
            _unit.land = Core.currentLand;
            _unit.team = _team.index;

            Core.worldMap.GetComponent<WorldMapCore>().RedrawMap();

            // add carni

            GameObject clone;

            for (i=0; i<=4; i++)
            {
                clone = Instantiate (Core.carniPrefab, _position, Quaternion.identity) as GameObject;

                if (index == Core.currentLand)
                {
                    Core.critters.Add(clone);
                }
                else
                {
                    critters.Add(clone);
                    clone.SetActive(false);
                }
                
                _team.members.Add(clone);
                _unit.members.Add(clone);

                clone.GetComponent<CritterCore>().team = Core.teams.Count-1;

                clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

                clone.GetComponent<CritterCore>().command = ECommand.guard;
                clone.GetComponent<CritterCore>().commandTarget = _team.members[0];
            }

            _team.members[0].GetComponent<CritterCore>().command = ECommand.none;
        }

        // spawn enemy group method

        public void SpawnEnemyGroup(Vector3 _position)
        {
        
        // add new unit

            Core.teams[2].units.Add(new Unit());
            Unit _unit = Core.teams[2].units[Core.teams[2].units.Count-1];
            Core.units.Add(_unit);
            _unit.land = Core.currentLand;
            _unit.team = 2;

            Core.worldMap.GetComponent<WorldMapCore>().RedrawMap();

            // add men

            GameObject clone;

            for (i=0; i<=4; i++)
            {
                clone = Instantiate (Core.manPrefab, _position, Quaternion.identity) as GameObject;

                if (index == Core.currentLand)
                {
                    Core.critters.Add(clone);
                }
                else
                {
                    critters.Add(clone);
                    clone.SetActive(false);
                }
                
                Core.teams[2].members.Add(clone);
                _unit.members.Add(clone);

                clone.GetComponent<CritterCore>().team = Core.teams.Count-1;

                clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

                clone.GetComponent<CritterCore>().command = ECommand.guard;
                clone.GetComponent<CritterCore>().commandTarget = Core.teams[2].members[0];
            }

            Core.teams[2].members[0].GetComponent<CritterCore>().command = ECommand.none;

            //

        }

        // spawn man method

        public GameObject SpawnMan(Vector3 _position, int _team)
        {
            GameObject clone;

            clone = Instantiate (Core.manPrefab, _position, Quaternion.identity) as GameObject;

            if (index == Core.currentLand)
            {
                Core.critters.Add(clone);
            }
            else
            {
                critters.Add(clone);
                clone.SetActive(false);
            }
            
            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);
            
            Core.teams[_team].members.Add(clone);
            clone.GetComponent<ManCore>().team = _team;
            clone.GetComponent<ManCore>().action = EAction.idle;

            return clone;
        }

        // spawn new tribe group

        public void SpawnNewTribeGroup()
        {
            Team t = new Team();

            Core.teams.Add(t);
            t.index = Core.teams.Count-1;
            t.name = "Tribe "+t.index;

            // generate random color

            Color c = new Color(
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f), 
            Random.Range(0f, 1f));

            t.color = c;

            Unit u = new Unit();

            t.units.Add(u);
            Core.units.Add(u);
            u.land = index;
            u.team = t.index;

            //if (Core.worldMap.activeSelf)
            //Core.worldMap.GetComponent<WorldMapCore>().RedrawMap();

            // add men

            GameObject clone;

            Vector3 p = new Vector3(Random.Range(0f,landPointX[landSections-2]),-100f,0f);

            for (i=0; i<=4; i++)
            {
                clone = Instantiate (Core.manPrefab, p, Quaternion.identity) as GameObject;

                if (index == Core.currentLand)
                {
                    Core.critters.Add(clone);
                }
                else
                {
                    critters.Add(clone);
                    clone.SetActive(false);
                }
                
                t.members.Add(clone);
                u.members.Add(clone);

                clone.GetComponent<CritterCore>().team = t.index;

                clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

                clone.GetComponent<CritterCore>().command = ECommand.guard;
                clone.GetComponent<CritterCore>().commandTarget = t.members[0];
            }

            t.members[0].GetComponent<CritterCore>().command = ECommand.none;


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
        public int   index;
        public string name;
        public Color color;
        public float attitude = 0f;

        public List<GameObject> members = new List<GameObject>();

        public List<Unit> units = new List<Unit>();
    }

    // ---------------------------------------

    public class Unit
    {
        // unit on the worldmap

        public int team;
        public int land;

        public List<GameObject> members = new List<GameObject>();
    }

    // ---------------------------------------

    public class WorldEvent
    {
        public float timeStamp;

        public bool executed = false;

        public EWorldEvent type = EWorldEvent.none;

    }

    //----------------------------------------------------------------------------------------------------------------

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

        // create first land and two nodes

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
        sprucePrefab = Instantiate (treePrefab, transform.position, transform.rotation) as GameObject;
        sprucePrefab.GetComponent<PlantCore>().plant = EPlant.spruce;
        sprucePrefab.GetComponent<PlantCore>().PlantInitialize();
        sprucePrefab.GetComponent<PlantCore>().BodyInitialize();
        sprucePrefab.GetComponent<PlantCore>().CreatePolygonCollider();
        sprucePrefab.GetComponent<PlantCore>().OptimizePolygonCollider(2.0f);
        sprucePrefab.SetActive(false); 
        
        // birch
        birchPrefab = Instantiate (treePrefab, transform.position, transform.rotation) as GameObject;
        Core.birchPrefab.GetComponent<PlantCore>().plant = EPlant.birch;
        Core.birchPrefab.GetComponent<PlantCore>().PlantInitialize();
        Core.birchPrefab.GetComponent<PlantCore>().BodyInitialize();
        Core.birchPrefab.GetComponent<PlantCore>().CreatePolygonCollider();
        Core.birchPrefab.GetComponent<PlantCore>().OptimizePolygonCollider(0.21f);
        Core.birchPrefab.SetActive(false); 
    }

    //-----------------------------------------------------

    public void LoadLand(int _index)
    {
        // save objects and deacivate them

        // save plants

        lands[currentLand].plants.Clear();
        
        for (i=0; i<plants.Count; i++)
        {
            lands[currentLand].plants.Add(plants[i]);
            plants[i].SetActive(false);
        }
        
        plants.Clear();

        // save items

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

        // save critters
        
        lands[currentLand].critters.Clear();

        critters.Remove(player); // <---- !
        
        for (i=0; i<critters.Count; i++)
        {
            lands[currentLand].critters.Add(critters[i]);
            critters[i].SetActive(false);
        }
        
        critters.Clear();

        // save structures

        lands[currentLand].structures.Clear();
        
        for (i=0; i<structures.Count; i++)
        {
            lands[currentLand].structures.Add(structures[i]);
            structures[i].SetActive(false);
        }

        structures.Clear();

        
        // save projects

        lands[currentLand].projects.Clear();
        
        for (i=0; i<projects.Count; i++)
        {
            lands[currentLand].projects.Add(projects[i]);
            projects[i].SetActive(false);
        }

        projects.Clear();
        
        // --------------------------
        
        // load a new land
        
        currentLand = _index;

        critters.Add(player); // <---- !

        landSections = lands[_index].landSections;

		for (i=0; i<landSections; i++)
		{
            landPointX[i] = lands[_index].landPointX[i];
            landPointY[i] = lands[_index].landPointY[i];
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

        for (i=0; i<lands[currentLand].structures.Count; i++)
        {
            structures.Add(lands[currentLand].structures[i]);
            lands[currentLand].structures[i].SetActive(true);
        }

        for (i=0; i<lands[currentLand].projects.Count; i++)
        {
            projects.Add(lands[currentLand].projects[i]);
            lands[currentLand].projects[i].SetActive(true);
        }

        // move members of player unit to the new land

        if (units.Count > 0)
        {
            for (i=0; i<teams[1].units[0].members.Count; i++)
            {
                if ((teams[1].units[0].members[i].GetComponent<CritterCore>().command == ECommand.guard)
                && (teams[1].units[0].members[i].GetComponent<CritterCore>().commandTarget == player)
                && (teams[1].units[0].members[i].GetComponent<CritterCore>().downed == false))
                {
                    lands[teams[1].units[0].members[i].GetComponent<BodyCore>().land].critters.Remove(teams[1].units[0].members[i]);
                    critters.Add(teams[1].units[0].members[i]);

                    teams[1].units[0].members[i].GetComponent<BodyCore>().land = _index;
                    teams[1].units[0].members[i].transform.position = player.transform.position;
                    teams[1].units[0].members[i].SetActive(true);

                }
            }
        }

        // set hp bars according to combat mode

        SetHpBarsActive(combatMode);

        // redraw ground and backgrounds

        ground.GetComponent<BackgroundCore>().Draw();
        background1.GetComponent<BackgroundCore>().Draw();
        background2.GetComponent<BackgroundCore>().Draw();
        background3.GetComponent<BackgroundCore>().Draw();
    }

    //-----------------------------------------------------
    /*
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

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        //mesh.SetUVs(0,uvs);

        mesh.RecalculateBounds();

        // THIS IS NOT REDRAW
        
    }
    */
    //-----------------------------------------------------
    /*
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
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i], 0));
                j++;
                verts[j] = (new Vector3(landPointX[i+1], landPointY[i+1], 0));
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

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));

            uvs.Add(new Vector2(0,0));
            uvs.Add(new Vector2(1,1));
            uvs.Add(new Vector2(1,0));
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris,0);
        //mesh.SetUVs(0,uvs);

        mesh.RecalculateBounds();
        
        // color
        
        Vector3[] vertices = mesh.vertices;
        Color[] colors = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        colors[i] = new Color(0.5f,0.5f,0.5f,1f);

        // assign the array of colors to the Mesh.
        mesh.colors = colors;

        
    }*/
    //-----------------------------------------------------

    void ExploreNodeL(int _a, int _b)
    {
        bool thisNodeExists = false;

        for (i=0; i<nodes.Count; i++)
        {
            if ((nodes[i].a == lands[currentLand].nodeL.a + _a)
            && (nodes[i].b == lands[currentLand].nodeL.b + _b))
            {
                // node already exists there

                thisNodeExists = true;
                
                // check if explored node is not attached to current land

                if ((nodes[i] != lands[currentLand].nodeR)
                && (nodes[i] != lands[currentLand].nodeL))
                {
                    // check if the node is visited

                    if (nodes[i].visited == false)
                    {
                        // if not, then generate new land
                        
                        land = new Land();
                        lands.Add(land);
                        land.index = lands.Count-1;
                        land.GenerateLand();

                        if ((_a == 1) || (_b == -1))
                        {
                            land.nodeL = nodes[i];
                            land.nodeR = lands[currentLand].nodeL;
                        }
                        else
                        {
                            land.nodeL = lands[currentLand].nodeL;
                            land.nodeR = nodes[i];
                        }

                    }
                        
                }
                
                break;
            } 
        }

        // if node not found

        if (thisNodeExists == false)  
        {
            // generate new node

            node = new Node();
            nodes.Add(node);
            node.a = lands[currentLand].nodeL.a + _a;
            node.b = lands[currentLand].nodeL.b + _b;

            // generate new land
            
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
                // node already exists there

                thisNodeExists = true;

                // check if explored node is not attached to current land

                if ((nodes[i] != lands[currentLand].nodeR)
                && (nodes[i] != lands[currentLand].nodeL))
                {
                    if (nodes[i].visited == false)
                    {
                        // generate new land
                
                        land = new Land();
                        lands.Add(land);
                        land.index = lands.Count-1;
                        land.GenerateLand();

                        if ((_a == 1) || (_b == -1))
                        {
                            land.nodeL = nodes[i];
                            land.nodeR = lands[currentLand].nodeR;
                        }
                        else
                        {
                            land.nodeL = lands[currentLand].nodeR;
                            land.nodeR = nodes[i];
                        }
                    }
                }
                
                break;
            }
        }

        if (thisNodeExists == false) // if node not found
        {
            // generate new node

            node = new Node();
            nodes.Add(node);
            node.a = lands[currentLand].nodeR.a + _a;
            node.b = lands[currentLand].nodeR.b + _b;

            // generate new land

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

    public void CreateWorldEvent(EWorldEvent _type, float _timeStamp)
    {
        WorldEvent _event = new WorldEvent();

        Core.events.Add(_event);
        _event.timeStamp = _timeStamp;
        _event.type = _type;

        switch (_type)
        {
            case EWorldEvent.carniEnter:
            {
                // ...

                break;
            }
        }
    }

    //-----------------------------------------------------

    public void ExecuteWorldEvent(WorldEvent _event)
    {
        if (_event.executed == false)
        {
            print(_event.type);

            switch (_event.type)
            {
                case EWorldEvent.carniEnter:
                {
                    Core.lands[currentLand].SpawnCarniPack(Core.mousePos);

                    _event.executed = true;
                    break;
                }

                case EWorldEvent.unitRandomMove:
                {
                    if (units.Count > 1)
                    {
                        // select random unit

                        int r = Random.Range(1,units.Count);

                        if (units[r].land != currentLand)
                        {
                            int a;  // destination land index
                            int b;
                            int j;

                            List<int> possibleDestinations = new List<int>(); // min. 3, max. 6 possible desinations
                            List<bool> enterThisDestinationFromTheRight = new List<bool>();

                            for (i=0; i<lands.Count; i++)
                            {
                                if ((lands[units[r].land].nodeR == lands[i].nodeL)
                                || (lands[units[r].land].nodeL == lands[i].nodeL))
                                {
                                    if ((possibleDestinations.Contains(lands[i].index) == false)
                                    && (lands[i].index != units[r].land))
                                    {
                                        possibleDestinations.Add(i);
                                        enterThisDestinationFromTheRight.Add(false);
                                    }
                                }

                                if ((lands[units[r].land].nodeR == lands[i].nodeR)
                                || (lands[units[r].land].nodeL == lands[i].nodeR))
                                {
                                    if ((possibleDestinations.Contains(lands[i].index) == false)
                                    && (lands[i].index != units[r].land))
                                    {
                                        possibleDestinations.Add(i);
                                        enterThisDestinationFromTheRight.Add(true);
                                    }

                                    // breakpoint
                                }
                            }

                            b = Random.Range(0, possibleDestinations.Count);
                            a = possibleDestinations[b];

                            possibleDestinations.Clear();
                            

                            for (i=0; i < units[r].members.Count; i++)
                            {
                                // move critter to the correct edge of the land
                                
                                if (enterThisDestinationFromTheRight[b] == false)
                                {
                                    units[r].members[i].transform.position = new Vector3(0f, -100f, transform.position.z);
                                }
                                else
                                {
                                    units[r].members[i].transform.position = new Vector3(500f, -100f, transform.position.z);
                                }

                                // delete critter and its carried bodies from one land

                                for (j = 0; j < units[r].members[i].GetComponent<CritterCore>().carriedBodies.Count; j++)
                                {
                                    lands[units[r].land].items.Remove(units[r].members[i].GetComponent<CritterCore>().carriedBodies[j]);
                                }

                                lands[units[r].land].critters.Remove(units[r].members[i]);

                                // change land for the critter and for carried bodies

                                units[r].members[i].GetComponent<PhysicalObject>().land = a;

                                for (j = 0; j < units[r].members[i].GetComponent<CritterCore>().carriedBodies.Count; j++)
                                {
                                    units[r].members[i].GetComponent<CritterCore>().carriedBodies[i].GetComponent<PhysicalObject>().land = a;
                                }

                                // add critter and its carried bodies to other land


                                if (a == currentLand)
                                {
                                    critters.Add(units[r].members[i]);
                                    units[r].members[i].SetActive(true);

                                    for (j = 0; j < units[r].members[i].GetComponent<CritterCore>().carriedBodies.Count; j++)
                                    {
                                        items.Add(units[r].members[i].GetComponent<CritterCore>().carriedBodies[j]);
                                        units[r].members[i].GetComponent<CritterCore>().carriedBodies[j].SetActive(true);
                                    }

                                }
                                else
                                {
                                    lands[a].critters.Add(units[r].members[i]);
                                }

                                //
                            }
                        
                            units[r].land = a;
                        
                            if (worldMap.activeSelf == true)
                            {
                                worldMap.GetComponent<WorldMapCore>().RedrawMap();
                            }
                        }
                    }

                    CreateWorldEvent(EWorldEvent.unitRandomMove,timeStamp+0.5f/24f);

                    _event.executed = true;
                    events.Remove(_event);

                    break;
                }

            }
        }
    }

    //-----------------------------------------------------

    void LeftMouseButton()
    {
        // ----- LEFT MOUSE BUTTON ------

        if (gamePaused == false)
        {

            if(Input.GetMouseButtonDown(0))
            {
                // if "choose from inventory" mode is enabled and no buttonI was clicked, then disable "choose from inventory" mode

                if ((chooseFromInventoryMode == true)
                && (mouseOverGUI == false))
                {
                    chooseFromInventoryMode = false;
                    InventoryManager(); // to clear colors from buttons type I
                }

                //

                // if combat mode is off

                if ((combatMode == false)
                && (mouseOverGUI == false))
                {
                    bool b = false;

                    // check if there is item under mouse to pick up / equip

                    if (rhit2D.Length > 0)
                    {
                        for (int i = 0; i<rhit2D.Length; i++)
                        {
                            if ((rhit2D[i].transform.gameObject.GetComponent<ItemCore>())
                            && (rhit2D[i].transform.gameObject.GetComponent<ItemCore>().isCarried == false))
                            {
                                // set player to pick up the first item found under mouse

                                player.GetComponent<CritterCore>().target  = rhit2D[i].transform.gameObject;
                                player.GetComponent<CritterCore>().targetX = rhit2D[i].transform.position.x;

                                player.GetComponent<CritterCore>().action = EAction.pickUp;

                                b = true;

                                break;
                            }
                        }
                    }

                    // if there is no item under mouse, then move player

                    if (b == false)
                    {
                        Vector3 v1;
                        v1 =  Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        player.GetComponent<CritterCore>().targetX = v1.x;
                        player.GetComponent<CritterCore>().action = EAction.move;
                        player.GetComponent<CritterCore>().target = null;

                        // move precisely, if targetX is very close

                        if (Mathf.Abs(player.transform.position.x - player.GetComponent<CritterCore>().targetX) < 2f)
                        player.GetComponent<CritterCore>().preciseMovement = true;
                        else
                        player.GetComponent<CritterCore>().preciseMovement = false;

                    }
                }

                //
            }

            if(Input.GetMouseButtonUp(0))
            {
                if (mouseOverGUI == false)
                ClearButtons();
            }

        }
    }

    //-----------------------------------------------------

    void RightMouseButton()
    {
        // ----- RIGHT MOUSE BUTTON ------

        if ((Input.GetMouseButtonDown(1))
        && (gamePaused == false))
        {
            if (combatMode == false)
            {
                ClearButtons();

                // check for new objects under mouse

                RMBManager();
            }
        }
    }

    //-----------------------------------------------------

    void HighlightObjectUnderMouse()
    {
        if (gamePaused == false)
        {
            // ---- HIGHLIGHTING OBJECTS UNDER MOUSE ----

            // 1. make previous objects white and keep alpha value

            if (rhit2D.Length > 0) 
            {
                for (i=0; i < rhit2D.Length; i++)
                {
                    if (rhit2D[i])
                    {
                        float f = rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color.a;

                        rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,f);
                    }
                }
            }

            // 2. find new objects under mouse

            rhit2D = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), new Vector2(0f,0f));
        
            // 3. make new objects colored and keep alpha value

            if (combatMode == false)
            {
                if (rhit2D.Length > 0)
                {
                    for (i=0; i < rhit2D.Length; i++)
                    {
                        float f = rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color.a;

                        rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.75f,0.75f,0.75f,f);

                    }
                }
            }
        }
    }

    //-----------------------------------------------------

    public void CreateButtonAList()
    {
        int ind = 0; // button index   

        if (!mouseOverGUI)
        {

            // if empty space was RMB-clicked

            if (RMBclickedObj == null)
            {
                /*
                if (gatheringPoint.activeSelf == false)
                {
                    AddButtonA(ind,"set gathering point",EAction.setGatheringPoint);
                    ind++;
                }
                else
                {
                    AddButtonA(ind,"delete gathering point",EAction.deleteGatheringPoint);
                    ind++;
                }
                */

                /*
                if (player.GetComponent<CritterCore>().carriedBodies.Count > 0)
                {
                    AddButtonA(i,"drop all here",EAction.dropAll);
                    ind++;
                }
                */
            }

            // if an object was RMB-clicked

            else
            {
                // item
        
                if (RMBclickedObj.GetComponent<ItemCore>())
                {   
                    if (RMBclickedObj.GetComponent<ItemCore>().isCarried == false)
                    {
                        // if any item

                        AddButtonA(ind,"pick up",EAction.pickUp,false);
                        ind++;

                        // if specific item

                        if (RMBclickedObj.GetComponent<ItemCore>().item == EItem.flint)
                        {
                            bool b = true;

                            if (player.GetComponent<ManCore>().hand1Slot)
                            {
                                if (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.hammerstone)
                                {
                                    b = false;
                                }
                            }
                            AddButtonA(ind,"craft",EAction.craftHandAxe,b);
                            ind++;
                        }
                        else
                        if ((RMBclickedObj.GetComponent<ItemCore>().item == EItem.bark)
                        || (RMBclickedObj.GetComponent<ItemCore>().item == EItem.barkTorch))
                        {
                            {
                                AddButtonA(ind,"set on fire",EAction.setOnFire,false);
                                ind++;
                            }
                        }
                        else
                        if (RMBclickedObj.GetComponent<ItemCore>().item == EItem.firewood)
                        {
                            if (player.GetComponent<ManCore>().hand1Slot)
                            {
                                if (player.GetComponent<ManCore>().hand1Slot.GetComponent<Burnable>())
                                {
                                    if (player.GetComponent<ManCore>().hand1Slot.GetComponent<Burnable>().fire)
                                    {
                                        AddButtonA(ind,"set on fire",EAction.setOnFire,false);
                                        ind++;
                                    }
                                }
                            }
                        }
                    }

                    // if item is carried by player

                    else if (RMBclickedObj.GetComponent<ItemCore>().carrier == player)
                    {
                        if (RMBclickedObj == player.GetComponent<ManCore>().hand1Slot)
                        {
                                    AddButtonA(ind,"unequip",EAction.unequip,false);
                                    ind++;
                        }
                        else if (RMBclickedObj.GetComponent<ItemCore>().isTool)
                        {
                                    AddButtonA(ind,"equip",EAction.equip,false);
                                    ind++;
                        }

                        AddButtonA(ind,"drop",EAction.drop,false);
                        ind++;
                    }



                }

                // fireplace

                else
                if (RMBclickedObj.GetComponent<FireplaceCore>())
                {
                    AddButtonA(ind,"put in fire",EAction.putItemInFireplace,false);
                    ind++;

                    if (RMBclickedObj.GetComponent<FireplaceCore>().itemInFire != null)
                    {
                        if (RMBclickedObj.GetComponent<FireplaceCore>().itemInFire.GetComponent<ItemCore>().item == EItem.flatRock)
                        {
                            AddButtonA(ind,"cook meat",EAction.heatItem,false);
                            ind++;
                        }
                    }

                }

                // other structures

                else
                if (RMBclickedObj.GetComponent<StructureCore>())
                {
                    // ...
                }

                // plant

                else
                if (RMBclickedObj.GetComponent<PlantCore>())
                {
                    // if rooted
            
                    if (RMBclickedObj.GetComponent<PlantCore>().isRooted == true)
                    {
                        bool b = true;

                        if (player.GetComponent<ManCore>().hand1Slot)
                        if (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>())
                        {
                            if ((player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                            || (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                            {
                                b = false;
                            }
                        }

                        GameObject o = AddButtonA(ind,"cut down",EAction.cutDown, b);
                        ind++;
                    }

                    // if not rooted

                    else 
                    {
                        if (RMBclickedObj.GetComponent<BodyCore>().isCarried)
                        {
                            if (RMBclickedObj.GetComponent<BodyCore>().carrier == player)
                            {
                                AddButtonA(ind,"drop",EAction.drop,false);
                                ind++;
                            }
                        }
                        else
                        {
                            AddButtonA(ind,"pick up",EAction.pickUp,false);
                            ind++;

                            if ((RMBclickedObj.GetComponent<PlantCore>().plant == EPlant.spruce)
                            || (RMBclickedObj.GetComponent<PlantCore>().plant == EPlant.birch))
                            {
                                bool b = true;

                                if (player.GetComponent<ManCore>().hand1Slot)
                                if (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>())
                                {
                                    if ((player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                                    || (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                                    {
                                        b = false;
                                    }
                                }

                                AddButtonA(ind,"process",EAction.processTree,b);
                                ind++;
                            }
                        }
                    }

                    // if specific plant

                    if ((RMBclickedObj.GetComponent<PlantCore>().plant == EPlant.grass)
                    && !(RMBclickedObj.GetComponent<BodyCore>().isCarried))
                    {
                        if (RMBclickedObj.GetComponent<PlantCore>().isRooted == false)
                        {
                            AddButtonA(ind,"craft cordage",EAction.craftCordage,false);
                            ind++;
                        }
                    }
                }

                // critter

                else
                if (RMBclickedObj.GetComponent<CritterCore>())
                {
                    if (RMBclickedObj.GetComponent<HerbiCore>())
                    {
                        if ((RMBclickedObj.GetComponent<CritterCore>().alive == false))
                        {
                            bool b = true;

                            if (player.GetComponent<ManCore>().hand1Slot)
                            if (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>())
                            {
                                if ((player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.sharpRock)
                                || (player.GetComponent<ManCore>().hand1Slot.GetComponent<ItemCore>().item == EItem.handAxe))
                                {
                                    b = false;
                                }
                            }

                            AddButtonA(ind,"obtain meat",EAction.obtainMeat,b);
                            ind++;
                        }
                    }
                    else
                    if (RMBclickedObj.GetComponent<ManCore>())
                    {
                        if ((RMBclickedObj.GetComponent<CritterCore>().alive == true)
                        && (RMBclickedObj != player))
                        {
                            AddButtonA(ind,"give item",EAction.giveItem,false);
                            ind++;

                            // wildman

                            if (RMBclickedObj.GetComponent<CritterCore>().team == 0)
                            {
                                AddButtonA(ind,"ask to join",EAction.convert,false);
                                ind++;
                            }
                        }
                    }
                }

                // project

                else
                if (RMBclickedObj.GetComponent<ProjectCore>())
                {
                    AddButtonA(ind,"continue",EAction.continueProject,false);
                    ind++;
                    AddButtonA(ind,"delete",EAction.deleteProject,false);
                    ind++;
                }
            }
        }
    }

    //-----------------------------------------------------

    void RMBManager()
    {
        if (rhit2D.Length > 0)
        RMBclickedObj = rhit2D[0].transform.gameObject;

        CreateButtonAList();

        if (rhit2D.Length > 0)
        {
            RMBclickedObj = rhit2D[0].transform.gameObject;

            for (i=0; i < rhit2D.Length; i++)
            {
                GameObject obj;
                obj = AddButtonO(i);
                obj.GetComponent<ButtonOCore>().image.GetComponent<Image>().sprite = rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().sprite;
                obj.GetComponent<ButtonOCore>().obj = rhit2D[i].transform.gameObject;
            }

            for (i=0; i < buttonsA.Count; i++)
            {
                buttonsA[i].GetComponent<ButtonACore>().isAnchoredToButtonO = true;
                buttonsA[i].GetComponent<ButtonACore>().anchorObject = buttonsO[0];

            }
        }
    }

    //-----------------------------------------------------

    GameObject AddButtonA(int _index, string _label, EAction _action, bool _darkened)
    {

        clone = Instantiate(buttonAPrefab, transform.position, transform.rotation) as GameObject;
        buttonsA.Add(clone);
        clone.transform.SetParent(myCanvas.transform,false);

        clone.GetComponent<ButtonACore>().pos = Camera.main.WorldToScreenPoint(Input.mousePosition/* + v3*/);

        clone.GetComponent<ButtonACore>().obj = RMBclickedObj;
        clone.GetComponent<ButtonACore>().index = _index;

        clone.GetComponentInChildren<Text>().text = _label;
        clone.GetComponent<ButtonACore>().action = _action;

        if (_darkened)
        {
            clone.GetComponent<Image>().color = new Color(0.15f, 0.15f, 0.15f, 1f);
            clone.GetComponent<ButtonACore>().text.GetComponent<Text>().color = Color.black;

            if (clone.GetComponent<ButtonACore>())
            clone.GetComponent<ButtonACore>().darkened = true;
        }

        return clone;
    }

    //-----------------------------------------------------

    public void InventoryManager() // <-- ManageInventory() would be better
    {
        chooseFromInventoryMode = false;

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

        if (player.GetComponent<ManCore>().hand1Slot != null)
        {
            AddButtonI(0,1);
        }

    }

    //-----------------------------------------------------

    public void AddButtonI(int _index, int _type)
    {
        clone = Instantiate(buttonIPrefab, transform.position, transform.rotation) as GameObject;
        buttonsI.Add(clone);
        clone.transform.SetParent(myCanvas.transform,false);

        clone2 = Instantiate(imagePrefab, transform.position, transform.rotation) as GameObject;
        clone2.transform.SetParent(clone.transform,false);

        if (_type == 0) // item in inventory
        {
            clone.transform.position = v5 + v6*_index - v6*0.5f*(player.GetComponent<CritterCore>().carriedBodies.Count-1);
            clone.GetComponent<ButtonICore>().index = _index;
            clone.GetComponent<ButtonICore>().obj = player.GetComponent<CritterCore>().carriedBodies[_index];

            clone2.GetComponent<Image>().sprite = player.GetComponent<CritterCore>().carriedBodies[_index].GetComponent<SpriteRenderer>().sprite;
        }
        else
        if (_type == 1) // hand1Slot slot
        {
            clone.transform.position = v5 + new Vector3(0f,600f,0f);
            clone.GetComponent<ButtonICore>().obj = player.GetComponent<ManCore>().hand1Slot;

            clone2.GetComponent<Image>().sprite = player.GetComponent<ManCore>().hand1Slot.GetComponent<SpriteRenderer>().sprite;
        }
    }

    //-----------------------------------------------------

    public GameObject AddButtonO(int _index)
    {
        clone = Instantiate(buttonOPrefab, transform.position, transform.rotation) as GameObject;
        buttonsO.Add(clone);
        clone.transform.SetParent(myCanvas.transform,false);

        clone.transform.position = Camera.main.WorldToScreenPoint(mousePos) + new Vector3(75f,-75f,0f) + v6*_index;
        clone.GetComponent<ButtonOCore>().worldPos = Camera.main.ScreenToWorldPoint(clone.transform.position);
        clone.GetComponent<ButtonOCore>().index = _index;

        clone2 = Instantiate(imagePrefab, transform.position, transform.rotation) as GameObject;
        clone2.transform.SetParent(clone.transform,false);
        clone.GetComponent<ButtonOCore>().image = clone2;

        return clone;
    }

    //-----------------------------------------------------

    public void SpawnPlayer()
    {
        if (player == null)
        {
            player = Instantiate (playerPrefab, transform.position + v1, transform.rotation) as GameObject;
            critters.Add(player);

            teams[1].members.Add(player);

        }
    }

    //-----------------------------------------------------

    public void SpawnMan(int _team)
    {
        clone = Instantiate (manPrefab, mousePos, transform.rotation) as GameObject;
        clone.name = "Man";
        clone.transform.position = new Vector3(clone.transform.position.x,clone.transform.position.y,0f);
        clone.GetComponent<ManCore>().team = _team;
        teams[_team].members.Add(clone);
        critters.Add(clone);

        if (_team == 1)
        {
            clone.GetComponent<CritterCore>().commandTarget = player;
            clone.GetComponent<CritterCore>().command = ECommand.guard;
        }

    }

    //-----------------------------------------------------

    public void SpawnHerbi()
    {
        clone = Instantiate (herbiPrefab, mousePos, transform.rotation) as GameObject;
        critters.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);
        teams[0].members.Add(clone);
        clone.GetComponent<HerbiCore>().action = EAction.idle;

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

    public GameObject SpawnStructure(EStructure _structure)
    {
        if (_structure == EStructure.campfire)
        clone = Instantiate (fireplacePrefab, mousePos, Quaternion.identity) as GameObject;

        // note: fireplace is a structure, specific fireplaces are: campfire, stove, kiln, furnace etc.

        else
        clone = Instantiate (structurePrefab, mousePos, Quaternion.identity) as GameObject;

        structures.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

        clone.GetComponent<StructureCore>().structure = _structure;

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
    /*
    public void CheckUIObjectsUnderMouse() // TEST
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        print(results.Count);
    }
    */
    //-----------------------------------------------------
    
    public float GetGroundY(float _x)
    {
        float groundY = 0f;

        for (i=1; i<GameCore.Core.landSections; i++)
	    {
		    if (_x < GameCore.Core.landPointX[i])
		    {
				int landSection = i;
				float landSteepness = Mathf.Atan2(landPointY[i]-landPointY[i-1],landPointX[i]-landPointX[i-1]);
		        groundY = landPointY[landSection-1] + (_x-landPointX[landSection-1]) * Mathf.Tan(landSteepness);

			    break;
		    }
	    }
                
        return groundY;
    }
    
    //-----------------------------------------------------
    
    public void ToggleHpBars()
    {
    
        for (int i = 0; i < critters.Count; i++)
        {
            if (critters[i].GetComponent<CritterCore>().hpBar)
            {
                critters[i].GetComponent<CritterCore>().hpBar.SetActive(!critters[i].GetComponent<CritterCore>().hpBar.activeSelf);

                if (critters[i].GetComponent<CritterCore>().hpBar.GetComponent<HpBar>().frame)
                critters[i].GetComponent<CritterCore>().hpBar.GetComponent<HpBar>().frame.SetActive(critters[i].GetComponent<CritterCore>().hpBar.activeSelf);
            }
        }
    }

    public void SetHpBarsActive(bool _b)
    {
        for (int i = 0; i < critters.Count; i++)
        {
            if (critters[i].GetComponent<CritterCore>().hpBar)
            {
                critters[i].GetComponent<CritterCore>().hpBar.SetActive(_b);

                if (critters[i].GetComponent<CritterCore>().hpBar.GetComponent<HpBar>().frame)
                critters[i].GetComponent<CritterCore>().hpBar.GetComponent<HpBar>().frame.SetActive(_b);
            }
        }
        
    }

    
    //-----------------------------------------------------
    
    public IEnumerator SunlightCoroutine()
    {
        for (;;)
        {
            // sunlight
        
            if (timeHour >= 18)
            {
                if (sunlight.GetComponent<Light>().intensity > 0)
                sunlight.GetComponent<Light>().intensity -= 0.004f;
                else
                sunlight.GetComponent<Light>().intensity = 0;
            }
            else
            if (timeHour >= 3)
            {
                if (sunlight.GetComponent<Light>().intensity < 1)
                sunlight.GetComponent<Light>().intensity += 0.004f;
                else
                sunlight.GetComponent<Light>().intensity = 1;
            }

            yield return new WaitForSeconds(1f);

            //
        }
    }

    //-----------------------------------------------------

    public void ClearButtons()
    {

        // clear buttons (type A)    

        for (i=0; i<=buttonsA.Count-1; i++)
        {
            Destroy(buttonsA[i]);
        }

        buttonsA.Clear();
        RMBclickedObj = null;
        


        // clear buttons (type O)

        for (i=0; i<buttonsO.Count; i++)
        {
            Destroy(buttonsO[i]);
        }

        buttonsO.Clear();



    }

    //-----------------------------------------------------

    public void PauseGame()
    {
        Time.timeScale = 0.00000000001f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        gamePaused = true;

    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        gamePaused = false;
    }

    // ============================================================ MAIN ======================================================================

    /// ---------------------- AWAKE ---------------------------

	void Awake()
	{
        Core = gameObject.GetComponent<GameCore>();

        // raycast

        rhit2D = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                            Camera.main.ScreenToWorldPoint(Input.mousePosition).y), new Vector2(0f,0f));
        //

        teams.Add(new Team()); // Team 0 -> Peaceful Team
        teams[0].index = 0;
        teams[0].name = "Nature";
        teams[0].color = Color.gray;

        teams.Add(new Team()); // Team 1 -> Player Team
        teams[1].index = 1;
        teams[1].name = "Blue Tribe";
        teams[1].color = Color.blue;

        teams.Add(new Team()); // Team 2 -> Bad Guys
        teams[2].index = 2;
        teams[2].name = "Dakini Tribe";
        teams[2].color = Color.red;

        InitializeIngamePrefabs();

        SetupFirstLand();

        SpawnPlayer();
        LoadLand(0);

        // create unit and add player to it

        Unit _unit;

        _unit = new Unit();

        teams[1].units.Add(_unit);
        units.Add(_unit);
        
        _unit.team = 1;
        _unit.land = currentLand;
        _unit.members.Add(player);

        //

        //CreateWorldEvent(EWorldEvent.unitRandomMove, 0.5f+0.5f/24f);

        hideHUD = true;
        
	}

    /// ---------------------------------------------------- START ---------------------------------------------------------

	void Start() 
	{
        Application.targetFrameRate = 60; // -1 for performance check (remember to turn v-sync off)

        timeHour = 12;
        timeDay = 1;
        timeStamp = timeHour/24f;

        sunlight = Instantiate(sunlightPrefab, new Vector3(0f,0f,0f), Quaternion.identity);

        // time scale
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        StartCoroutine("SunlightCoroutine");

        SetHpBarsActive(false);
        combatMode = false;

        v5 = new Vector3(800f,80f,0f);

	}

    /// ---------------------------------------------------- UPDATE ----------------------------------------------------------
     
	void Update()
	{
        // mouse position
        
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // time

        timePrevious = Time.time;

        // 1 real second = 1 minute in game

        timeMinute += Time.deltaTime/1f; // 1 day and night cycle = 24 real minutes
        timeStamp += Time.deltaTime/1f * 0.000694444444444f; 
                                        // the number is 1/1140 -> one divided by number of minutes in a day. This number
                                        // multiplied by timeIncrement (in minutes) gives the correct timeStamp increment (in days)
        if (timeMinute >= 60f)
        {
            timeHour++;
            timeMinute -= 60f;

            if (timeHour == 24)
            {
                timeHour = 0;
                timeDay++;
            }
        }

        // check and execute world events

        for (i=0; i<events.Count; i++)
        {
            if (events[i].executed == false)
            {
                if (timeStamp >= events[i].timeStamp)
                {
                    ExecuteWorldEvent(events[i]);
                }
            }
        }



        // input
        
        if (Input.GetKeyDown(KeyCode.S))
		SpawnItem(EItem.stoneSpear);

        if (Input.GetKeyDown(KeyCode.F))
		SpawnItem(EItem.handAxe);

        if (Input.GetKeyDown(KeyCode.I))
		SpawnItem(EItem.barkTorch);

        if (Input.GetKeyDown(KeyCode.K))
        {
            clone = SpawnStructure(EStructure.campfire);
            clone.GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_firewood;
            clone.transform.localScale = new Vector3(0.6f,0.6f,0.6f);

            SpawnItem(EItem.meat);
            SpawnItem(EItem.flatRock);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            player.GetComponent<PhysicalObject>().MessageText("Hello world.");
        }

        if (Input.GetKeyDown(KeyCode.H))
        hideHUD = !hideHUD;

        if (Input.GetKeyDown(KeyCode.V))
        //lands[currentLand].SpawnEnemyGroup(mousePos);
        //lands[currentLand].SpawnCarniPack(mousePos);
        {
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.P))
        SpawnPlatform();

        //
        
        if (Input.GetKeyDown(KeyCode.M))
        SpawnMan(2);

        if (Input.GetKeyDown(KeyCode.N))
        SpawnMan(1);

        if (Input.GetKeyDown(KeyCode.O))
        {
            worldMap.SetActive(!worldMap.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
		{
            escMenu.SetActive(!escMenu.activeSelf);

            if (helpPanel.activeSelf)
            {
                helpPanel.SetActive(false);
            }

            if (escMenu.activeSelf)
            PauseGame();
            else
            UnpauseGame();
        }


        // world map scrolling

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (worldMap.activeSelf)
            {
                worldMap.GetComponent<WorldMapCore>().cx += 10f;
                worldMap.GetComponent<WorldMapCore>().RedrawMap();
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (worldMap.activeSelf)
            {
                worldMap.GetComponent<WorldMapCore>().cx -= 10f;
                worldMap.GetComponent<WorldMapCore>().RedrawMap();
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (worldMap.activeSelf)
            {
                worldMap.GetComponent<WorldMapCore>().cy += 10f;
                worldMap.GetComponent<WorldMapCore>().RedrawMap();
            }
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (worldMap.activeSelf)
            {
                worldMap.GetComponent<WorldMapCore>().cy -= 10f;
                worldMap.GetComponent<WorldMapCore>().RedrawMap();
            }
        }

        //

        if (Input.GetKeyDown(KeyCode.B))
        {
            buildPanel.SetActive(!buildPanel.activeSelf);
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.GetComponent<CritterCore>().hp = player.GetComponent<CritterCore>().hpMax;
            player.GetComponent<CritterCore>().downed = false;
            player.GetComponent<CritterCore>().alive = true;
            player.transform.rotation = Quaternion.identity;
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale += 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            Time.timeScale -= 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            combatMode = !combatMode;
            ToggleHpBars();

            // clear existing buttons of type A

            for (i=0; i<buttonsA.Count; i++)
            {
                Destroy(buttonsA[i]);
            }

            buttonsA.Clear();
            RMBclickedObj = null;

            // clear existing buttons of type O

            for (i=0; i<buttonsO.Count; i++)
            {
                Destroy(buttonsO[i]);
            }

            buttonsO.Clear();

            //

        }

        // switch to fullscreen (works only in standalone)
		if (Input.GetKeyDown(KeyCode.F))
		Screen.fullScreen = !Screen.fullScreen;
        //

        HighlightObjectUnderMouse();
        LeftMouseButton();
        RightMouseButton();

        // ----------------------------

        // gui text

        if (guiText)
        {
            if (hideHUD == true)
            {
                guiText.text = "";
                consoleText.text = "";
            }
            else
            {
            consoleText.text = consoleOutput;

            guiText.text = "\n Time in game: " + timeHour + ":" + Mathf.FloorToInt(timeMinute)+
                            "\n Time.timeScale: " + Time.timeScale+
                            "\n Time.time: " + Time.time+
                            "\n Time Stamp: " + timeStamp+
                            "\n Day: " + timeDay+
                            "\n Lands: " + lands.Count.ToString()+
                            "\n Units: " + units.Count.ToString()+
                            "\n\n Plants: " + plants.Count.ToString()+
                            "\n Structures: " + structures.Count.ToString()+
                            "\n Critters: " + critters.Count.ToString()+
                            "\n Items: " + items.Count.ToString()+
                            "\n\n mouseOverGUI: " + mouseOverGUI+
                            "\n chooseFromInventoryMode: " + chooseFromInventoryMode+
                            "\n player.chosenObject: " + chosenObject+
                            "\n sunlight intensity: " + sunlight.GetComponent<Light>().intensity+
                            "\n rhit2D.Length: " + rhit2D.Length+
                            "\n RMBclickedObj: " + RMBclickedObj+
                            "\n chosenObject: " + chosenObject+
                            "\n gatheringPointX: " + gatheringPointX+
                            "\n zoom: " + Camera.main.GetComponent<CameraCore>().zoom+
                            "\n targetZoom: " + Camera.main.GetComponent<CameraCore>().targetZoom;
            }
        }

        // combat mode text

        if (combatModeText)
        {
            if (hideHUD == true)
            {
                combatModeText.text = "";
            }
            else
            {
                if (combatMode == false)
                combatModeText.text = "Combat mode OFF";
                else
                combatModeText.text = "Combat mode ON";
            }
        }

        // draw targetX lines

        //if (hideHUD == false)
        //{
            for (var i=1; i<critters.Count; i++)
            {
                Debug.DrawLine(critters[i].transform.position, new Vector3(critters[i].GetComponent<CritterCore>().targetX, GetGroundY(critters[i].GetComponent<CritterCore>().targetX) ,0f) );
            }

            Debug.DrawLine(player.transform.position, new Vector3(player.GetComponent<CritterCore>().targetX, GetGroundY(player.GetComponent<CritterCore>().targetX) ,0f) );

        //}

        // gathering point sprite

        if (gatheringPoint.activeSelf == true)
        {
            gatheringPoint.transform.position = new Vector3(gatheringPointX, GetGroundY(gatheringPointX), 0f);
        }


    }

    /// -------------------------- LATE UPDATE --------------------------

    // ---------------------------------------------- WORLD TRAVEL ------------------------------------------------
    
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
    
}