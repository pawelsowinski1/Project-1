using System.Collections;
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
    	BodyInitialize();
        timerMove = 1;
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

    void FixedUpdate()
    {
        AI();
    }

}
