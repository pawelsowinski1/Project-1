using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlant {none, hemp, tree, berry_bush};

public class PlantCore : BodyCore
{
    public EPlant plant;

    void PlantInitialize()
    {
        switch (plant)
        {
            case EPlant.tree:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_tree;
                break;
            }
            case EPlant.hemp:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_hemp;
                transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                break;
            }
            case EPlant.berry_bush:
            {
                GetComponent<SpriteRenderer>().sprite = GameCore.Core.spr_berry_bush;
                transform.localScale = new Vector3(0.8f,0.8f,0.8f);
                break;
            }
        }
    }

	void Start ()
    {
		BodyInitialize();
        PlantInitialize();
        type = TypeEnum.tree;
    }
	
	void Update ()
    {
		CalculateLand();
		PlaceOnGround();
	}

    void OnDestroy()
    {
        GameCore.Core.plants.Remove(gameObject); // possible bug here
    }
}