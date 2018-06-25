using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantCore : BodyCore
{
	void Start ()
    {
		BodyInitialize();
        type = TypeEnum.tree;
    }
	
	void Update ()
    {
		CalculateLand();
		PlaceOnGround();
	}

    void OnDestroy()
    {
        GameCore.Core.plants.Remove(gameObject);
    }
}
