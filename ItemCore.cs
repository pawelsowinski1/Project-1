// 26-07-2017

using UnityEngine;
using System.Collections;

public class ItemCore : BodyCore
{
    // ================ ITEM CORE ======================

    // parent class:  BodyCore
    // child classes: -

    // =================================================

	void Start()
	{
		BodyInitialize();

        int i = 2;
        GetComponent<SpriteRenderer>().sortingLayerID = i;
	}

	void Update() 
	{
		CalculateLand();
		PlaceOnGround();
	}
}
