// 27-11-2017

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ENUM

enum ItemEnum {wood, meat, stone};

//

public class GameCore : MonoBehaviour 
{

    // ================== GAME CORE =====================

	public int     landSections;
	public float[] landPointX;
	public float[] landPointY;
    public int     lands;

    public GameObject playerPrefab;
    public GameObject manPrefab;
    public GameObject itemPrefab;
    public GameObject pantsPrefab;
    public GameObject hpBarPrefab;
    public GameObject labelPrefab;

    public GameObject player;

    GameObject clone;
    GameObject clone2;

    public List<GameObject> critters = new List<GameObject>();
    public List<GameObject> items    = new List<GameObject>();

    public const float GRAVITY = 10f;

    Vector3 v1 = new Vector3(75f,0,0);  // man spawn position
    Vector3 v2 = new Vector3(0,0.4f,0); // pants position

    /// GenerateLand()
    /// DrawLand()
    /// SpawnPlayer()
    /// SpawnMan(1)
    /// SpawnItem()

    // ==================================================

    

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

    //_____________________________________________________

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

    //_____________________________________________________

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

            clone = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
            clone.GetComponent<LabelCore>().parent = player;
            clone.GetComponent<TextMesh>().text = player.GetComponent<CritterCore>().label;
        }
    }

    //______________________________________________

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
        //clone2.GetComponent<TextMesh>().text = "Wojownik";
    }
    //______________________________________________

    void SpawnItem()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        clone = Instantiate (itemPrefab, mousePos, transform.rotation) as GameObject;
        items.Add(clone);
        clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y, 0f);

        clone2 = Instantiate(labelPrefab, transform.position, transform.rotation) as GameObject;
        clone2.GetComponent<LabelCore>().parent = clone;
        clone2.GetComponent<TextMesh>().text = clone.GetComponent<ItemCore>().label;
    }

    // ==================================================

    /// ----- AWAKE -----

	void Awake()
	{
        lands = 2;

        int i;

        for (i=1; i<=lands; i++)
        {
            GenerateLand();
        }

        SpawnPlayer();
        SpawnItem();

        /*
        SpawnMan(1);
        SpawnMan(1);
        SpawnMan(1);
        SpawnMan(1);
        SpawnMan(1);

        SpawnMan(0);
        SpawnMan(0);
        SpawnMan(0);
        SpawnMan(0);
        */
	}

    /// ----- START -----

	void Start () 
	{
        //Application.targetFrameRate = -1; // for performance check (remember to turn v-sync off)
	}


    /// ----- UPDATE -----

	void Update()
	{
        DrawLand();

        if (Input.GetKeyDown(KeyCode.I))
		SpawnItem();

        // works only in standalone
        
		if (Input.GetKeyDown(KeyCode.F))
		Screen.fullScreen = !Screen.fullScreen;

        // ---
	}

    /// ----- FIXED UPDATE -----

	void FixedUpdate()
	{	


    }
}
