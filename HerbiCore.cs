using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerbiCore : CritterCore
{
    // ========= HERBI CORE ===========

    // parent class:  CritterCore
    // child classes: -

    int timerMove = 0;

	void Start () 
    {
    	BodyInitialize();

        label = name;
	}
	
	void Update ()
    {
    	CalculateLand();
		PlaceOnGround();
		DamageColorize();

        if (isCarried == true)
        {
            if (carrier != null)
            {
                transform.position = carrier.transform.position + new Vector3(0,0.6f,0);
            }
        }
	}
}
