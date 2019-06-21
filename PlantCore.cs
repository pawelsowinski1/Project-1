using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Collider2DOptimization;

public enum EPlant {none, birch, hemp, spruce, berryBush};

public class PlantCore : BodyCore
{
    public EPlant plant;

    public float stock;
    public float age;
    public bool rooted = true;

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
        age = Random.Range(10f,100f);

        //GetComponent<SpriteRenderer>().sortingOrder = 20;

        switch (plant)
        {
            case EPlant.spruce:
            {
                name = "Spruce";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_spruce;
                transform.localScale = new Vector3(age*0.015f,age*0.015f,age*0.015f);
                type = EType.tree;
                

                break;
            }
            case EPlant.birch: 
            {
                name = "Birch";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_birch;
                transform.localScale = new Vector3(age*0.015f,age*0.015f,age*0.015f);
                type = EType.tree;

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
                transform.localScale = new Vector3(1f,1f,1f);
                break;
            }
        }

        if (type == EType.tree)
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
        rooted = false;
        transform.Rotate(0,0,45f);
    }

    // =============================================== MAIN LOOP ===================================================

	void Start ()
    {
        kind = EKind.plant;

		BodyInitialize();
        PlantInitialize();
    }
	
	void Update ()
    {
		CalculateLand();
		PlaceOnGround();

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