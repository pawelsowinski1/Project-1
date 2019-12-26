using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarniCore : CritterCore
{
    // ========= HERBI CORE ===========

    // A herbivore.

    // parent class:  CritterCore
    // child classes: -

	void Start () 
    {
        name = "Carni";
        hp = 100f;
    
    	BodyInitialize();
        timerMove = 1;
	}
	
	void Update ()
    {
		Gravity();
		DamageColorize();
	}

    void FixedUpdate()
    {
        AI();
    }

}
