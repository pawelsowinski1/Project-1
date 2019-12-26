using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Collider2DOptimization;

public enum EPlant {none, birch, hemp, spruce, berryBush, grass};

public class PlantCore : BodyCore
{
    public EPlant plant;

    public float size; // 10f - 100f
    public bool isRooted = true;

    Collider2D colli;
    PolygonColliderOptimizer opti;

    public Collider2D CreateBoxCollider()
    {
        colli = gameObject.AddComponent<BoxCollider2D>();
        colli.isTrigger = true;
        return colli;
    }

    public Collider2D CreatePolygonCollider()
    {
        colli = gameObject.AddComponent<PolygonCollider2D>();
        colli.isTrigger = true;
        return colli;
    }

    public void OptimizePolygonCollider(double _tolerance)
    {
        opti = gameObject.AddComponent<PolygonColliderOptimizer>();
        opti.tolerance = _tolerance;
        opti.Optimize();
        Destroy(opti);
    }
    

    public void PlantInitialize()
    {
        size = Random.Range(10f,100f);

        switch (plant)
        {
            case EPlant.spruce:
            {
                name = "Spruce";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_spruce;
                transform.localScale = new Vector3(size*0.015f,size*0.015f,size*0.015f);

                break;
            }

            case EPlant.birch: 
            {
                name = "Birch";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_birch;
                transform.localScale = new Vector3(size*0.015f,size*0.015f,size*0.015f);

                break;
            }

            case EPlant.hemp:
            {
                name = "Hemp";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_hemp;
                transform.localScale = new Vector3(0.8f,0.8f,0.8f);

                break;
            }

            case EPlant.berryBush:
            {
                name = "Bush";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_berry_bush;
                transform.localScale = new Vector3(1f+size*0.005f,1f+size*0.005f,1f+size*0.005f);
                break;
            }

            case EPlant.grass:
            {
                name = "Grass";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_grass;
                transform.localScale = new Vector3(0.025f+size*0.02f,0.025f+size*0.02f,0.025f+size*0.02f);

                break;
            }

        }

        if ((plant == EPlant.spruce)
        || (plant == EPlant.birch))
        Destroy(GetComponent<BoxCollider2D>());
        else
        {
            // resize box collider 2D to fit the sprite; 
            Vector2 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
            gameObject.GetComponent<BoxCollider2D>().size = S;
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2 (0, (S.y / 2));
            //
        }

    }

    public void Uproot()
    {
        isRooted = false;
        transform.Rotate(0,0,45f);
    }

    // =============================================== MAIN LOOP ===================================================

	void Start()
    {
		BodyInitialize();
        PlantInitialize();
    }
	
	void Update()
    {
		Gravity();

        // enable being carried

        if (isCarried == true)
        {
            if (carrier != null)
            {
                transform.position = carrier.transform.position + new Vector3(0,0.6f,0);
            }
        }

        //
	}

    void OnDestroy()
    {
        GameCore.Core.plants.Remove(gameObject); 
    }
}