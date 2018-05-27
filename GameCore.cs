using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameCore : MonoBehaviour
{
    // ============= GAME CORE ================

    // ------------- VARIABLES ----------------

    public int     currentLand;
	public int     landSections;
	public float[] landPointX;
	public float[] landPointY;

    public GameObject playerPrefab;
    public GameObject manPrefab;
    public GameObject herbiPrefab;
    public GameObject itemPrefab;
    public GameObject pantsPrefab;
    public GameObject hpBarPrefab;
    public GameObject labelPrefab;
    public GameObject button;

    static public GameCore Core; // game core

    public GameObject player;

    GameObject clone;
    GameObject clone2;

    public List<Land> lands          = new List<Land>();
    public List<GameObject> critters = new List<GameObject>();
    public List<GameObject> items    = new List<GameObject>();

    public const float GRAVITY = 10f; // todo

    public RaycastHit2D[] rhit2D;

    Vector3 v1 = new Vector3(75f,0,0);  // player spawn position
    Vector3 v2 = new Vector3(0,0.4f,0); // pants relative position
    Vector3 v3 = new Vector3(80f,-20f,0); // button on RMB relative position

    Vector3 RMBclickPos = new Vector3(0f,0f,0f);

    // ------------ CLASSES ------------------

    public class Land
    {
        public int     landSections;
        public float[] landPointX;
	    public float[] landPointY;

        public void GenerateLand()
        {
		    landSections = 100;

		    landPointX = new float[landSections];
		    landPointY = new float[landSections];

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

        }
    }

    //  ---------------- METHODS ------------------

    /// GenerateLand()
    /// LoadLand()
    /// DrawLand()

    /// SpawnPlayer()
    /// SpawnMan(1)
    /// SpawnHerbi()
    /// SpawnItem(1)
    /// SpawnTree()

    //-----------------------------------------------------

    void GenerateLand()
    {
		landSections = 100;

		landPointX = new float[landSections];
		landPointY = new float[landSections];

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
    }

    //-----------------------------------------------------

    void LoadLand(int land_)
    {
        currentLand = land_;

        landSections = lands[land_].landSections;

		int i;

		for (i=0; i<landSections; i++)
		{
            landPointX[i] = lands[land_].landPointX[i];
            landPointY[i] = lands[land_].landPointY[i];
		}

    }


    //-----------------------------------------------------

    void DrawLand()
    {
		Vector3 startPoint = new Vector3 (0, 0, 0);
		Vector3 endPoint = new Vector3 (0, 0, 0);

		int i;

		for (i=1; i<landSections; i++)
		{
			startPoint.Set (landPointX[i-1],landPointY[i-1],0);
			endPoint.Set (landPointX[i],landPointY[i],0);
			
			Debug.DrawLine(startPoint, endPoint, Color.red);
		}
    }

    //-----------------------------------------------------

    void SpawnPlayer()
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

            // label

            //clone = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
            //clone.GetComponent<LabelCore>().parent = player;
            //clone.GetComponent<TextMesh>().text = player.GetComponent<CritterCore>().label;
        }
    }

    //-----------------------------------------------------

    void SpawnMan(int team_)
    {
        clone = Instantiate (manPrefab,transform.position + v1,transform.rotation) as GameObject;
        clone.GetComponent<ManCore>().team = team_;
        critters.Add(clone);

        // pants
        
        clone2 = Instantiate (pantsPrefab,transform.position, transform.rotation) as GameObject;
        clone2.transform.parent = clone.transform;
        clone2.transform.position = clone.transform.position + v2;

        if (team_ == 0)
        clone2.GetComponent<SpriteRenderer>().color = Color.blue;
        else if (team_ == 1)
        clone2.GetComponent<SpriteRenderer>().color = Color.red;

        // hp bar

        clone2 = Instantiate(hpBarPrefab, transform.position, transform.rotation) as GameObject;
        clone2.GetComponent<HpBarCore>().parent = clone;

        // label

        //clone2 = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
        //clone2.GetComponent<LabelCore>().parent = clone;
        //clone2.GetComponent<TextMesh>().text = clone.GetComponent<BodyCore>().label;
    }
    //-----------------------------------------------------

    public void SpawnHerbi()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        clone = Instantiate (herbiPrefab, mousePos, transform.rotation) as GameObject;
        critters.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

        // hp bar

        clone2 = Instantiate(hpBarPrefab, transform.position, transform.rotation) as GameObject;
        clone2.GetComponent<HpBarCore>().parent = clone;

        // label

        //clone2 = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
        //clone2.GetComponent<LabelCore>().parent = clone;
        //clone2.GetComponent<TextMesh>().text = clone.GetComponent<BodyCore>().label;
    }

    //-----------------------------------------------------


    void SpawnItem(ItemEnum e)
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        clone = Instantiate (itemPrefab, mousePos, transform.rotation) as GameObject;
        items.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

        clone.GetComponent<ItemCore>().item = e;

        // label

        //clone2 = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
        //clone2.GetComponent<LabelCore>().parent = clone;
        //clone2.GetComponent<TextMesh>().text = clone.GetComponent<ItemCore>().label;
    }

    //-----------------------------------------------------

    void SpawnTree() // -> SpawnPlant
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        clone = Instantiate (itemPrefab, mousePos, transform.rotation) as GameObject;
        items.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

        // label

        //clone2 = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
        //clone2.GetComponent<LabelCore>().parent = clone;
        //clone2.GetComponent<TextMesh>().text = clone.GetComponent<ItemCore>().label;        
    }

    // ==================== MAIN LOOP =======================

    /// ----- AWAKE -----

	void Awake()
	{
        GenerateLand(); // <- Change this. Random land generation is not needed here, array initialization would be enough. 

        // todo

        Land land;

        land = new Land();
        lands.Add(land);

        land = new Land();
        lands.Add(land);

        land = new Land();
        lands.Add(land);

        lands[0].GenerateLand();
        lands[1].GenerateLand();
        lands[2].GenerateLand();

        // raycast initialization
        rhit2D = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), new Vector2(0f,0f));

        //

        LoadLand(1);
        SpawnPlayer();
	}

    /// ----- START -----

	void Start () 
	{
        //Application.targetFrameRate = -1; // for performance check (remember to turn v-sync off)

        button = GameObject.Find("Button");
        button.SetActive(false);
	}

    /// ----- UPDATE -----

	void Update()
	{
        DrawLand();

        //

        if(Input.GetMouseButtonDown(1))
        {
            if (button.activeSelf == false)
            {
                button.SetActive(true);
                RMBclickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + v3);
                button.transform.position = Camera.main.WorldToScreenPoint(RMBclickPos);
            }
            else
            button.SetActive(false);
        }

        if (button.activeSelf == true)
        {
            button.transform.position = Camera.main.WorldToScreenPoint(RMBclickPos);
        }

        print (button.transform.position);

        //

        if (Input.GetKeyDown(KeyCode.Alpha0))
		LoadLand(0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
		LoadLand(1);

        if (Input.GetKeyDown(KeyCode.Alpha2))
		LoadLand(2);

        if (Input.GetKeyDown(KeyCode.I))
		SpawnItem(ItemEnum.flint_blade);

        if (Input.GetKeyDown(KeyCode.M))
        SpawnMan(1);

        if (Input.GetKeyDown(KeyCode.N))
        SpawnMan(0);

        if (Input.GetKeyDown(KeyCode.O))
        landPointY[50] -= 1;

        if (Input.GetKeyDown(KeyCode.H))
        SpawnHerbi();

        // works only in standalone
		if (Input.GetKeyDown(KeyCode.F))
		Screen.fullScreen = !Screen.fullScreen;
        //

        // ---- HIGHLITING OBJECTS UNDER MOUSE ----

        int i;

        // 1. make previous objects white

        if (rhit2D.Length > 0)
        {
            for (i=0; i < rhit2D.Length; i++)
            {
                // BUG HERE !
                // NullReferenceException: Object reference not set to an instance of an object.
                rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,1f);
            }
        }
        // 2. find new objects under mouse

        rhit2D = Physics2D.RaycastAll(new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,Camera.main.ScreenToWorldPoint(Input.mousePosition).y), new Vector2(0f,0f));
        
        // 3. make new objects red

        if (rhit2D.Length > 0)
        {
            //objectUnderCursor = rhit2D[0].transform.gameObject;

            for (i=0; i < rhit2D.Length; i++)
            {
                rhit2D[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.7f,0f,0f,1f);
            }
        }
        else
        {
            //if (objectUnderCursor != null)
            //objectUnderCursor.GetComponent<SpriteRenderer>().color = new Color(0.7f,0f,0f,1f);

            //objectUnderCursor = null;
        }

        // -------------------
	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{
    }
}
