﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbiCore : CritterCore
{
    // ========= HERBI CORE ===========

    // A herbivore.

    // parent class:  CritterCore
    // child classes: -

	void Start () 
    {
        name = "Herbi";
        type = EType.herbi;
        team = 0;
        hp = 30f;
    
    	BodyInitialize();
        timerMove = 1;
	}
	
	void Update ()
    {
    	CalculateLand();
		PlaceOnGround();
		DamageColorize();

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

    void FixedUpdate()
    {
        AI();
    }

}
