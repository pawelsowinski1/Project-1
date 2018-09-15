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

        switch (plant)
        {
            case EPlant.spruce:
            {
                name = "Spruce";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_spruce;
                transform.localScale = new Vector3(age*0.02f,age*0.02f,age*0.02f);
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
                name = "Berry bush";
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_berry_bush;
                transform.localScale = new Vector3(0.8f,0.8f,0.8f);
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

    // =============================================== MAIN LOOP ===================================================

	void Start ()
    {
		BodyInitialize();
        PlantInitialize();
    }
	
	void Update ()
    {
		CalculateLand();
		PlaceOnGround();
	}

    void OnDestroy()
    {
        GameCore.Core.plants.Remove(gameObject); // possible bug here //<-- nope.

    }
}